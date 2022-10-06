using Core;
using System;
using UnityEngine;

namespace Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected string entityID;
        [SerializeField] protected string team;

        private Vector3 spawnPosition;

        public Vector3 SpawnPos => spawnPosition;
        public string EntityID => entityID;
        public string Team { get => team; set { team = value; } }
        
        public bool IsTank => entityID.Contains(GameManager.TANK_ID);
        public bool IsProjectile => entityID.Contains(GameManager.PROJECTILE_ID);
        public bool IsItem => entityID.Contains(GameManager.ITEM_ID);

        public bool BruteForceAddToTeam;

        protected virtual void Start()
        {
            if (BruteForceAddToTeam)
            {
                EntityManager.emInstance.GetTeam(team).entities.Add(gameObject);
            }

            spawnPosition = transform.position;
        }
    }
}
