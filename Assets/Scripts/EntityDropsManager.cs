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

        private void Start()
        {
            damageManager.OnDamageTaken += OnDamageTaken;
        }

        private void OnDamageTaken(string attackerID, string tankID, float prevHealth, float currentHealth)
        {
            EntityDropListSO.DropList dropList = dropListSO.EntityDropList.Find(x => x.tankID == tankID);
            if (dropList != null)
            {
                Transform location = entityManager.GetEntityByID(tankID).transform;
                foreach (GameObject entityDrop in dropList.entityDrops)
                {
                    GameObject loot = Instantiate(entityDrop, location.position, Quaternion.identity);
                }
            }
        }
    }
}