using Core;
using System;
using UnityEngine;

namespace Entities
{
    public class BaseManagerClass : MonoBehaviour
    {
        [SerializeField] protected string entityID;
        [SerializeField] protected string team;

        public string EntityID => entityID;
        public string Team { get => team; set { team = value; } }

        public bool BruteForceAddToTeam;

        protected virtual void Start()
        {
            if (BruteForceAddToTeam)
            {
                EntityManager.instance.GetTeam(team).entities.Add(gameObject);
            }
        }
    }
}
