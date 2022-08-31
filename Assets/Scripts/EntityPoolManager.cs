using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EntityPoolManager : PoolManager
    {
        public Action<GameObject> OnEntityPulled;
        public Action<GameObject> OnEntityPushed;
        public static EntityPoolManager entityPoolInstance;
        protected override void Awake()
        {
            entityPoolInstance = this;
        }

        public override GameObject PullFromPool(GameObject poolRep, Action<GameObject> method)
        {
            string poolName = poolRep.name;
            GameObject poolItem;
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
            OnEntityPulled?.Invoke(poolItem);
            
            return poolItem;
        }

        public override void PushToPool(GameObject poolItem)
        {
            base.PushToPool(poolItem);
            UnsubscribeEvents(poolItem);
            OnEntityPushed?.Invoke(poolItem);
        }

        public void DoEventListenChecks(GameObject toPull)
        {

        }

        public void UnsubscribeEvents(GameObject toPush)
        {
            
        }
    }
}