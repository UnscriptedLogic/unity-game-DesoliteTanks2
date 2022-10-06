using System;
using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entities.Inventory;

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

        public void OnEntityDeath(Entity entity)
        {
            if (entity.GetComponent<IInventoryAsLoot>() != null)
            {
                EntityInventory entityInventory = entity.GetComponent<EntityInventory>();

                for (int i = 0; i < entityInventory.InventorySlots.Count; i++)
                {
                    for (int j = 0; j < entityInventory.InventorySlots[i].amount; j++)
                    {
                        CreateLoot(entity.transform.position, entityInventory.InventorySlots[i].item.ItemScriptable.ItemObject);
                    }

                }

                entityInventory.Clear();
            }

            if (dropListSO.LootTableExists(entity.EntityID, out EntityDropListSO.DropList dropList))
            {
                Vector3 entityLocation = entity.transform.position;
                foreach (GameObject entityDrop in dropListSO.GetLootFromTable(dropList, entity.EntityID))
                {
                    CreateLoot(entityLocation, entityDrop);
                }
            }
        }

        public void CreateLoot(Vector3 entityLocation, GameObject entityDrop)
        {
            GameObject loot = EntityManager.emInstance.CreateEntity(entityDrop, "Items");
            loot.transform.SetPositionAndRotation(entityLocation + Vector3.up, Quaternion.identity);
            float dirX = UnityEngine.Random.Range(-splashRadius, splashRadius);
            float dirZ = UnityEngine.Random.Range(-splashRadius, splashRadius);
            Vector3 direction = entityLocation + new Vector3(dirX, splashHeight, dirZ);
            loot.SetActive(true);
            loot.GetComponent<Rigidbody>().AddForce((direction - entityLocation).normalized * force, ForceMode.Impulse);
        }
    }
}