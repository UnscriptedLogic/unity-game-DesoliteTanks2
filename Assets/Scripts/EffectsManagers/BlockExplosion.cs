using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{
    public class BlockExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject blockExplosion;
        [SerializeField] private float explosionForce = 100f;
        [SerializeField] private Rigidbody[] rbs;

        protected Vector3[] positions;
         
        private void Awake()
        {
            positions = new Vector3[rbs.Length];
            for (int i = 0; i < rbs.Length; i++)
            {
                positions[i] = rbs[i].transform.localPosition;
            }
        }

        private void OnEnable()
        {
            foreach (Rigidbody rigidbody in rbs)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.AddExplosionForce(explosionForce, transform.position, 10f);
            }
            
            EffectsManager.instance.CreateParticle(blockExplosion, transform.position, transform.rotation, 0.5f);
        }

        private void OnDisable()
        {
            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].transform.localPosition = positions[i];
            }
        }
    }
}