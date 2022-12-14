using Core;
using TMPro;
using System;
using Entities;
using UnityEngine;
using Entities.Spawning;
using System.Collections.Generic;

namespace LevelManagement
{
    [Serializable]
    public class WL_Spawn
    {
        public GameObject entity;
        public int amountToSpawn;
        public float entitySpawnInterval;
        public float completeDelay;
    }

    [Serializable]
    public class WL_SpawnList
    {
        public List<WL_Spawn> spawnList;
        public float entityGroupSpawnInterval;
        public bool WaitsForClearedLevel;
    }

    public class WaveLevelContext : LevelStateContext, IListensToEntityDeath
    {
        [Header("Preparation")]
        [SerializeField] protected float startDelay = 5f;
        [SerializeField] protected EntityManager entityManager;
        protected WLDetailsSO wLDetailsSO;

        [Header("Spawning Settings")]
        [SerializeField] private Vector2 spawningArea;
        [SerializeField] private Vector3 center;
        [SerializeField] private SpawningManager spawningManager;
        [SerializeField] private List<WL_SpawnList> wl_SpawnLists = new List<WL_SpawnList>();
        [SerializeField] private bool drawGizmos;

        [Header("User Interface")]
        [SerializeField] private EntityScoreManager scoreManager;
        [SerializeField] private GameObject endGamePage;
        [SerializeField] private TextMeshProUGUI endGameText;
        [SerializeField] private TextMeshProUGUI timePlayedTMP;
        [SerializeField] private TextMeshProUGUI coinsCollectedTMP;
        [SerializeField] private TextMeshProUGUI enemiesKilledTMP;
        [SerializeField] private TextMeshProUGUI playerDeathTMP;
        [SerializeField] private TextMeshProUGUI finalScoreTMP;

        private int waveIndex = -1;
        private WaveLevelFactory waveLevelFactory;

        public int WaveIndex { get => waveIndex; set { waveIndex = value; } }
        public float StartDelay => startDelay;
        public Vector2 SpawnArea => spawningArea;
        public Vector3 Center => center;
        public WLDetailsSO LevelDetails { get => wLDetailsSO; set { wLDetailsSO = value; } }
        public SpawningManager SpawnManager => spawningManager;
        public List<WL_SpawnList> WL_SpawnList { get => wl_SpawnLists; set { wl_SpawnLists = value; } }
        public GameObject EndGamePage => endGamePage;
        public TextMeshProUGUI EndGameText { get => endGameText; set { endGameText = value; } }
        public EntityScoreManager ScoreManager => scoreManager;
        public EntityManager EntityManager => entityManager;
        public TextMeshProUGUI TimePlayedTMP => timePlayedTMP;
        public TextMeshProUGUI CoinsCollectedTMP => coinsCollectedTMP;
        public TextMeshProUGUI EnemiesKilledTMP => enemiesKilledTMP;
        public TextMeshProUGUI PlayerDeathTMP => playerDeathTMP;
        public TextMeshProUGUI FinalScoreTMP => finalScoreTMP;

        private void Start()
        {
            entityManager.OnEntityDeath += OnEntityDeath;

            waveLevelFactory = new WaveLevelFactory(this);
            StartStateMachine(waveLevelFactory.SetUp());
        }

        public override void StateAfterSetUp()
        {
            currLevelState = waveLevelFactory.CountDown();
            currLevelState.EnterState();
        }

        private void OnDrawGizmos()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, new Vector3(spawningArea.x, 0f, spawningArea.y));
        }

        //Switches regardless of any state
        public void OnEntityDeath(Entity entity)
        {
            if (entity.EntityID.Contains("base"))
            {
                currLevelState.SwitchState(waveLevelFactory.EndGame(false));
            }
        }
    }
}