using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EntityPoolManager : PoolManager
    {
        public EntityManager entityManager;
        
        public static EntityPoolManager entityPoolInstance;

        protected override void Awake()
        {
            entityPoolInstance = this;
        }

        protected override void Start()
        {

        }

        public override GameObject PullFromPool(GameObject poolRep, Action<GameObject> method)
        {
            string poolName = poolRep.name;
            GameObject poolItem;

            if (pools == null)
            {
                prePools = new List<Pool>();
                pools = new Dictionary<string, Queue<GameObject>>();
            }

            if (pools.ContainsKey(poolName))
            {
                if (pools[poolName].Count > 1)
                {
                    poolItem = pools[poolName].Dequeue();
                }
                else
                {
                    poolItem = CreatePoolItem(poolRep);
                }

            }
            else
            {
                CreatePool(poolName, poolRep);
                poolItem = CreatePoolItem(poolRep);
            }

            method(poolItem);
            DoEventListenChecks(poolItem);
            
            return poolItem;
        }

        public override void PushToPool(GameObject poolItem)
        {
            base.PushToPool(poolItem);
            UnsubscribeEvents(poolItem);
        }

        public void DoEventListenChecks(GameObject toPull)
        {
            if (toPull.GetComponent<IListensToEntityDeath>() != null)
            {
                entityManager.OnEntityDeath += toPull.GetComponent<IListensToEntityDeath>().OnEntityDeath;
            }
        }

        public void UnsubscribeEvents(GameObject toPush)
        {
            if (toPush.GetComponent<IListensToEntityDeath>() != null)
            {
                entityManager.OnEntityDeath -= toPush.GetComponent<IListensToEntityDeath>().OnEntityDeath;
            }
        }
    }
}