using System;
using UnityEngine;
using LevelManagement;
using System.Collections.Generic;
using Entities.Inventory;

namespace Core
{
    public class EntityPoolManager : PoolManager
    {
        public EntityManager entityManager;
        public EntityScoreManager scoreManager;
        public LevelStateContext levelStateContext;
        public DamageManager damageManager;
        public InventoryManager inventoryManager;

        public static EntityPoolManager entityPoolInstance;

        protected override void Awake()
        {
            entityPoolInstance = this;
        }

        protected override void Start()
        {

        }

        public override GameObject PullFromPool(GameObject objectPrefab, Action<GameObject> method)
        {
            string poolName = objectPrefab.name;
            GameObject poolItem;

            if (pools == null)
            {
                prePools = new List<Pool>();
                pools = new Dictionary<string, Transform>();
            }

            if (pools.ContainsKey(poolName))
            {
                if (pools[poolName].childCount > 0)
                {
                    poolItem = pools[poolName].GetChild(0).gameObject;
                    poolItem.transform.SetParent(null);
                }
                else
                {
                    poolItem = CreatePoolItem(objectPrefab);
                }

            }
            else
            {
                CreatePool(poolName, objectPrefab);
                poolItem = CreatePoolItem(objectPrefab);
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

        public void DoEventListenChecks(GameObject entity)
        {
            IListensToEntityDeath[] entityDeathListeners = entity.GetComponents<IListensToEntityDeath>();
            for (int i = 0; i < entityDeathListeners.Length; i++)
            {
                entityManager.OnEntityDeath += entityDeathListeners[i].OnEntityDeath;
            }

            IListensToEntityCreated[] entityCreatedListeners = entity.GetComponents<IListensToEntityCreated>();
            for (int i = 0; i < entityCreatedListeners.Length; i++)
            {
                entityManager.OnEntityCreated += entityCreatedListeners[i].OnEntityCreated;
            }

            IRequiresScoreManager[] requiresScoreManagers = entity.GetComponents<IRequiresScoreManager>();
            for (int i = 0; i < requiresScoreManagers.Length; i++)
            {
                requiresScoreManagers[i].InitScoreManager(scoreManager);
            }

            IListensToGameState[] gameStateListeners = entity.GetComponents<IListensToGameState>();
            for (int i = 0; i < gameStateListeners.Length; i++)
            {
                levelStateContext.OnEndGameStateChanged += gameStateListeners[i].OnGameStateChanged;
            }

            IListensToOnDamage[] damageTakenListeners = entity.GetComponents<IListensToOnDamage>();
            for (int i = 0; i < damageTakenListeners.Length; i++)
            {
                damageManager.OnDamageTaken += damageTakenListeners[i].OnAnyEntityTookDamage;
            }

            IListensToInventoryUpdate[] inventoryUpdateListeners = entity.GetComponents<IListensToInventoryUpdate>();
            for (int i = 0; i < inventoryUpdateListeners.Length; i++)
            {
                inventoryManager.OnInventoryUpdated += inventoryUpdateListeners[i].OnInventoryUpdated;
            }
        }

        public void UnsubscribeEvents(GameObject entity)
        {
            IListensToEntityDeath[] entityDeathListeners = entity.GetComponents<IListensToEntityDeath>();
            for (int i = 0; i < entityDeathListeners.Length; i++)
            {
                entityManager.OnEntityDeath -= entityDeathListeners[i].OnEntityDeath;
            }

            IListensToEntityCreated[] entityCreatedListeners = entity.GetComponents<IListensToEntityCreated>();
            for (int i = 0; i < entityCreatedListeners.Length; i++)
            {
                entityManager.OnEntityCreated -= entityCreatedListeners[i].OnEntityCreated;
            }

            IListensToGameState[] gameStateListeners = entity.GetComponents<IListensToGameState>();
            for (int i = 0; i < gameStateListeners.Length; i++)
            {
                levelStateContext.OnEndGameStateChanged -= gameStateListeners[i].OnGameStateChanged;
            }

            IListensToOnDamage[] damageTakenListeners = entity.GetComponents<IListensToOnDamage>();
            for (int i = 0; i < damageTakenListeners.Length; i++)
            {
                damageManager.OnDamageTaken -= damageTakenListeners[i].OnAnyEntityTookDamage;
            }

            IListensToInventoryUpdate[] inventoryUpdateListeners = entity.GetComponents<IListensToInventoryUpdate>();
            for (int i = 0; i < inventoryUpdateListeners.Length; i++)
            {
                inventoryManager.OnInventoryUpdated -= inventoryUpdateListeners[i].OnInventoryUpdated;
            }
        }
    }
}