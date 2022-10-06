using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct ClipSettings
{
    public float volume;
    public bool loop;
    public bool playOnAwake;
    public float spatialBlend;
    public float minDistance;
    public float maxDistance;

    public ClipSettings(float volume = 0.5f, bool loop = false, float spatialBlend = 1f, float minDistance = 10f, float maxDistance = 20f, bool playOnAwake = false)
    {
        this.volume = volume;
        this.loop = loop;
        this.spatialBlend = spatialBlend;
        this.minDistance = minDistance;
        this.maxDistance = maxDistance;
        this.playOnAwake = playOnAwake;
    }
}

public enum ClipPriority
{
    LOW,
    MEDIUM,
    HIGH,
    ABSOLUTE
}

public class AudioTicket
{
    public float timeLeft;
    public bool loopsAudio;
    public GameObject audioObject;
    public EffectsManager effectsManager;

    private bool hasPlayed;

    public AudioTicket(float timeLeft, GameObject audioObject, EffectsManager effectsManager, bool loopsAudio)
    {

        this.timeLeft = timeLeft;
        this.audioObject = audioObject;
        this.effectsManager = effectsManager;
        this.loopsAudio = loopsAudio;
        hasPlayed = false;
    }

    public void TimeCheck()
    {
        if (!hasPlayed)
        {
            audioObject.SetActive(true);
            audioObject.GetComponent<AudioSource>().Play();
            hasPlayed = true;
        }

        if (loopsAudio)
        {
            return;
        }

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
            effectsManager.RemoveAudioTicket(this);
    }
}

[Serializable]
public class VFXSettings
{
    public AudioClip audioClip;
    public float volume;
    public float spatialBlend = 1f;
    public bool loop;
    public bool playOnAwake;
    public ClipPriority clipPriority;

    [Space(15)]
    public GameObject particle;
    public float particleDuration;

    private AudioTicket ticketRef;
    private GameObject particleRef;

    public AudioTicket AudioTicketRef => ticketRef;
    public GameObject ParticleRef => particleRef;
    public AudioSource audioSource => ticketRef.audioObject.GetComponent<AudioSource>();

    public bool isAudioPlaying => ticketRef != null;

    public void PlayVFX(Vector3 location, Quaternion rotation, Transform parent = null)
    {
        if (audioClip != null)
        {
            ticketRef = EffectsManager.instance.PlaySound(audioClip, location, new ClipSettings(volume: volume, spatialBlend: spatialBlend, loop: loop, playOnAwake: playOnAwake), clipPriority, parent);
        }

        if (particle != null)
        {
            particleRef = EffectsManager.instance.CreateParticle(particle, location, rotation, particleDuration);
        }
    }

    public void StopVFX()
    {
        if (ticketRef != null)
        {
            EffectsManager.instance.RemoveAudioTicket(ticketRef);
        }

        if (particleRef != null)
        {
            EffectsManager.instance.RemoveVFX(particleRef);
        }

        ticketRef = null;
        particleRef = null;
    }
}

public class EffectsManager : MonoBehaviour
{
    [Range(0f, 100f)][SerializeField] private float masterVolume = 100f;
    [SerializeField] private GameObject audioObject;
    [SerializeField] private int maxSoundEffects;
    private List<AudioTicket> audioTickets = new List<AudioTicket>();
    private List<GameObject> existingVFXs = new List<GameObject>();

    private Queue<AudioTicket> highPriority = new Queue<AudioTicket>();
    private Queue<AudioTicket> mediumPriority = new Queue<AudioTicket>();
    private Queue<AudioTicket> lowPriority = new Queue<AudioTicket>();

    public static EffectsManager instance;
    public void Awake() => instance = this;

    public AudioTicket PlaySound(AudioClip clip, Vector3 position, ClipSettings clipSettings, ClipPriority clipPriority = ClipPriority.LOW, Transform parent = null)
    {
        if (clipPriority != ClipPriority.ABSOLUTE)
        {
            if (audioTickets.Count >= maxSoundEffects)
            {
                return null;
            } 
        }

        AudioTicket audioTicket = null;
        EntityPoolManager.entityPoolInstance.PullFromPool(audioObject, createdAudioObject =>
        {
            AudioSource audioSource = createdAudioObject.GetComponent<AudioSource>();
            audioSource.playOnAwake = clipSettings.playOnAwake;
            audioSource.clip = clip;
            audioSource.volume = clipSettings.volume / 100 * masterVolume;
            audioSource.loop = clipSettings.loop;
            audioSource.spatialBlend = clipSettings.spatialBlend;
            audioSource.minDistance = clipSettings.minDistance;
            audioSource.maxDistance = clipSettings.maxDistance;

            createdAudioObject.transform.position = position;
            createdAudioObject.transform.parent = parent;

            audioTicket = new AudioTicket(clip.length, createdAudioObject, this, clipSettings.loop);
            switch (clipPriority)
            {
                case ClipPriority.LOW:
                    lowPriority.Enqueue(audioTicket);
                    break;
                case ClipPriority.MEDIUM:
                    mediumPriority.Enqueue(audioTicket);
                    break;
                case ClipPriority.HIGH:
                    highPriority.Enqueue(audioTicket);
                    break;
                case ClipPriority.ABSOLUTE:
                    audioTickets.Add(audioTicket);
                    break;
                default:
                    break;
            }
        });

        return audioTicket;
    }
    
    public GameObject CreateParticle(GameObject particlePrefab, Vector3 position, Quaternion rotation, float duration = 1f)
    {
        return EntityPoolManager.entityPoolInstance.PullFromPool(particlePrefab, createdParticle =>
        {
            createdParticle.name = particlePrefab.name;
            createdParticle.transform.position = position;
            createdParticle.transform.rotation = rotation;
            createdParticle.transform.SetParent(null);
            
            EntityPoolManager.entityPoolInstance.PushToPoolAfter(duration, createdParticle);
            createdParticle.SetActive(true);
            existingVFXs.Add(createdParticle);
        });
    }

    public void RemoveAudioTicket(AudioTicket audioTicket)
    {
        if (audioTicket == null)
        {
            return;
        }

        //This is my custom object pooling manager. You can just do Destroy(audioTicket.audioObject) instead
        EntityPoolManager.entityPoolInstance.PushToPool(audioTicket.audioObject); 
        audioTickets.Remove(audioTicket);
    }

    public void RemoveVFX(GameObject particleToRemove)
    {
        EntityPoolManager.entityPoolInstance.PushToPool(particleToRemove);
        existingVFXs.Remove(particleToRemove);
    }

    private void Update()
    {
        if (audioTickets.Count < maxSoundEffects)
        {
            if (highPriority.Count > 0)
            {
                audioTickets.Add(highPriority.Dequeue());
            }
            else if (mediumPriority.Count > 0)
            {
                audioTickets.Add(mediumPriority.Dequeue());
            }
            else if (lowPriority.Count > 0)
            {
                audioTickets.Add(lowPriority.Dequeue());
            }
        }

        if (audioTickets.Count > 0)
        {
            for (int i = 0; i < audioTickets.Count; i++)
            {
                audioTickets[i].TimeCheck();
            }
        }
    }
}
