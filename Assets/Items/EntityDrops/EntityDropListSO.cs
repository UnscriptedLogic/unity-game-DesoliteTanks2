using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Entities.EntityDropListSO;

namespace Entities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/New Entity Drop List", fileName = "New Entity Drop List")]
    public class EntityDropListSO : ScriptableObject
    {
        [Serializable]
        public class LootDrop
        {
            public GameObject loot;
            public float chance;
        }

        [Serializable]
        public class DropList
        {
            public string tankID;
            public List<LootDrop> entityDrops = new List<LootDrop>();
        }

        [SerializeField] private List<DropList> dropList = new List<DropList>();
        public List<DropList> EntityDropList => dropList;

        public bool LootTableExists(string entityID, out DropList entityDropList)
        {
            entityDropList = dropList.Find(x => entityID.Contains(x.tankID));
            return entityDropList != null;
        }

        public GameObject[] GetLootFromTable(DropList entityDropList, string entityID, int rolls = 1)
        {
            List<GameObject> loot = new List<GameObject>();
            for (int roll = 0; roll < rolls; roll++)
            {
                for (int i = 0; i < entityDropList.entityDrops.Count; i++)
                {
                    float randomizeChance = UnityEngine.Random.Range(0f, 100f);
                    if (randomizeChance <= entityDropList.entityDrops[i].chance)
                    {
                        loot.Add(entityDropList.entityDrops[i].loot);
                    }
                }
            }

            return loot.ToArray();
        }

        public GameObject[] GetLoot(string entityID, int rolls = 1)
        {
            List<GameObject> loot = new List<GameObject>();
            DropList entityDropList = dropList.Find(x => entityID.Contains(x.tankID));
            for (int roll = 0; roll < rolls; roll++)
            {
                for (int i = 0; i < entityDropList.entityDrops.Count; i++)
                {
                    float randomizeChance = UnityEngine.Random.Range(0f, 100f);
                    if (randomizeChance <= entityDropList.entityDrops[i].chance)
                    {
                        loot.Add(entityDropList.entityDrops[i].loot);
                    }
                }
            }

            return loot.ToArray();
        }
    }
}