using CameraManagement;
using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class PlayerRespawnManager : MonoBehaviour, IListensToEntityDeath
    {
        [SerializeField] private string entityID;
        [SerializeField] private float respawnDelay;

        [Header("Components")]
        [SerializeField] private EntityManager entityManager;
        [SerializeField] private Transform playerStart;

        private void Start()
        {
            entityManager.OnEntityDeath += OnEntityDeath;
        }

        public void OnEntityDeath(Entity entity)
        {
            if (entity.EntityID.Contains(entityID))
            {
                StartCoroutine(RespawnEntity(entity.gameObject, entity.Team, respawnDelay));
            }
        }

        private IEnumerator RespawnEntity(GameObject entity, string entityTeam, float spawnDelay)
        {
            yield return new WaitForSeconds(spawnDelay);
            Debug.Log("Respawned!");
            GameObject respawnedPlayer = entityManager.CreateEntity(entity, entityTeam);
            respawnedPlayer.transform.SetPositionAndRotation(entity.GetComponent<Entity>().SpawnPos, Quaternion.Euler(Vector3.zero));

            respawnedPlayer.SetActive(true);
        }
    }
}