using System;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EntityDropsManager : MonoBehaviour, IListensToEntityDeath
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
            entityManager.OnEntityDeath += OnEntityDeath;
        }

        public void OnEntityDeath(GameObject entity)
        {
            Debug.Log("Hello");

            string entityID = entity.GetComponent<BaseManagerClass>().EntityID;
            EntityDropListSO.DropList dropList = dropListSO.EntityDropList.Find(x => entityID.Contains(x.tankID));
            if (dropList != null)
            {
                Vector3 entityLocation = entity.transform.position;
                foreach (GameObject entityDrop in dropList.entityDrops)
                {
                    GameObject loot = EntityManager.instance.CreateEntity(entityDrop, entity.GetComponent<BaseManagerClass>().Team);
                    loot.transform.SetPositionAndRotation(entityLocation + Vector3.up, Quaternion.identity);
                    float dirX = UnityEngine.Random.Range(-splashRadius, splashRadius);
                    float dirZ = UnityEngine.Random.Range(-splashRadius, splashRadius);
                    Vector3 direction = entityLocation + new Vector3(dirX, splashHeight, dirZ);
                    loot.SetActive(true);
                    loot.GetComponent<Rigidbody>().AddForce((direction - entityLocation).normalized * force, ForceMode.Impulse);
                }
            }
        }
    }
}