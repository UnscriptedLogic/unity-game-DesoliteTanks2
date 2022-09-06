using System;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EntityDropsManager : MonoBehaviour
    {
        [SerializeField] private EntityDropListSO dropListSO;
        [SerializeField] private DamageManager damageManager;
        [SerializeField] private EntityManager entityManager;

        [Header("Splash")]
        [SerializeField] private float splashRadius;
        [SerializeField] private float splashHeight;
        [SerializeField] private float force;

        private void Start()
        {
            damageManager.OnDamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(string attackerID, string tankID, float prevHealth, float currentHealth)
        {
            EntityDropListSO.DropList dropList = dropListSO.EntityDropList.Find(x => x.tankID == tankID);
            if (dropList != null)
            {
                Vector3 entityLocation = entityManager.GetDeadEntityByID(tankID).transform.position;
                foreach (GameObject entityDrop in dropList.entityDrops)
                {
                    GameObject loot = Instantiate(entityDrop, entityLocation + Vector3.up, Quaternion.identity);

                    float dirX = UnityEngine.Random.Range(-splashRadius, splashRadius);
                    float dirZ = UnityEngine.Random.Range(-splashRadius, splashRadius);
                    Vector3 direction = entityLocation + new Vector3(dirX, splashHeight, dirZ);

                    loot.GetComponent<Rigidbody>().AddForce((direction - entityLocation).normalized * force, ForceMode.Impulse);
                }
            }
        }
    }
}