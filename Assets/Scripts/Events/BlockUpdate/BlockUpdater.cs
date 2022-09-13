using Core;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid.Pathfinding
{
    public struct BlockDetails
    {
        public string entityID;
        public string teamName;
        public Vector3 position;
        public Vector3 rotation;

        public BlockDetails(string entityID, string teamName, Vector3 position, Vector3 rotation)
        {
            this.entityID = entityID;
            this.teamName = teamName;
            this.position = position;
            this.rotation = rotation;
        }
    }

    public class BlockUpdater : MonoBehaviour, IListensToEntityDeath
    {
        public Action<BlockDetails> OnBlockCreated;
        public Action<BlockDetails> OnBlockDestroyed;

        [SerializeField] private EntityManager entityManager;

        public static BlockUpdater blockUpdaterInstance;
        private void Awake()
        {
            blockUpdaterInstance = this;
        }

        private void Start()
        {
            entityManager.OnEntityDeath += OnEntityDeath;
        }

        public void OnEntityDeath(GameObject entity)
        {
            BaseManagerClass baseManagerClass = entity.GetComponent<BaseManagerClass>();
            string id = baseManagerClass.EntityID;

            if (id.Contains("block"))
            {
                OnBlockDestroyed?.Invoke(new BlockDetails(id, baseManagerClass.Team, entity.transform.position, entity.transform.rotation.eulerAngles));
            }
        }
    }
}