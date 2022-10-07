using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public class EntityScoreManager : MonoBehaviour, IListensToEntityCreated, IListensToEntityDeath
    {
        [SerializeField] private string entityToTrackID;
        [SerializeField] private string entityOpponentTeamName;
        [SerializeField] private EntityManager entityManager;
        private Dictionary<string, int> teamScores = new Dictionary<string, int>();

        private int enemiesKilled;
        private int playerDeaths;
        private int coinsCollected;
        private float timePlayed;
        private int finalScore;

        public Action<Dictionary<string, int>> OnScoreModified;

        public float TimePlayed => timePlayed;
        public int EnemiesKilled => enemiesKilled;
        public int PlayerDeaths => playerDeaths;
        public int CoinsCollected => coinsCollected;
        public int FinalScore => finalScore;

        protected void Awake()
        {
            entityManager.OnEntityCreated += OnEntityCreated;
            entityManager.OnEntityDeath += OnEntityDeath;
        }

        private void Update()
        {
            timePlayed += Time.deltaTime;
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

        public void OnEntityDeath(Entity entity)
        {
            if (entity.EntityID.Contains(entityToTrackID))
            {
                playerDeaths++;
            }
            
            if (entity.Team.Contains(entityOpponentTeamName) && entity.IsTank)
            {
                enemiesKilled++;
            }

            if (entity.EntityID.Contains("coin"))
            {
                coinsCollected++;
            }
        }

        public void CalculateScores()
        {
            finalScore = (10 * coinsCollected) + (100 * enemiesKilled) - (100 * playerDeaths);
        }
    }
}