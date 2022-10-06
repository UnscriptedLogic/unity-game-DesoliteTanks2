using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class EntityScoreManager : MonoBehaviour, IListensToEntityCreated
    {
        [SerializeField] private EntityManager entityManager;
        private Dictionary<string, int> teamScores = new Dictionary<string, int>();

        public Action<Dictionary<string, int>> OnScoreModified;

        protected void Awake()
        {
            entityManager.OnEntityCreated += OnEntityCreated;
        }

        public void OnEntityCreated(GameObject entity)
        {
            for (int i = 0; i < entityManager.entityTeams.Count; i++)
            {
                string teamName = entityManager.entityTeams[i].teamName;
                if (!teamScores.ContainsKey(teamName))
                {
                    teamScores.Add(teamName, 0);
                }
            }
        }

        public void AddScore(string team, int amount)
        {
            if (!teamScores.ContainsKey(team))
            {
                teamScores.Add(team, amount);
            }
            else
            {
                teamScores[team] += amount;
            }

            OnScoreModified?.Invoke(teamScores);
        }
    }
}