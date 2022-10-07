using Entities.Spawning;
using SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LevelManagement
{
    public class MainMenuContext : LevelStateContext
    {
        [Header("Main Menu Extension")]
        [SerializeField] private Vector2 spawningArea;
        [SerializeField] private Vector3 center;
        [SerializeField] private float spawnDelay;
        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private SpawningManager spawningManager;
        [SerializeField] private List<GameObject> entitiesToSpawn = new List<GameObject>();

        [Header("Wave Level Select")]
        [SerializeField] private GameObject levelButtonPrefab;
        [SerializeField] private List<WLDetailsSO> waveLevelDetails;
        [SerializeField] private Transform waveLevelParent;
        [SerializeField] private GameObject levelSelect;

        [Header("Wave Level View")]
        [SerializeField] private GameObject levelView;
        [SerializeField] private WaveLevelView waveLevelView;

        private MainMenuFactory mainMenuFactory;

        public GameObject LevelSelectPage => levelSelect;
        public GameObject LevelViewPage => levelView;
        public WaveLevelView WaveLevelView => waveLevelView;
        public GameObject LevelButtonPrefab => levelButtonPrefab;
        public SceneChanger SceneChanger => sceneChanger;
        public Transform WaveLevelParent => waveLevelParent;
        public List<WLDetailsSO> WLDetails => waveLevelDetails;
        public SpawningManager SpawningManager => spawningManager;
        public List<GameObject> EntitiesToSpawn => entitiesToSpawn;
        public float SpawnDelay => spawnDelay;
        public Vector2 SpawnArea => spawningArea;
        public Vector3 Center => center;

        private void Start()
        {
            mainMenuFactory = new MainMenuFactory(this);
            StartStateMachine(mainMenuFactory.SetUp());
        }

        public override void StateAfterSetUp()
        {
            currLevelState = mainMenuFactory.SpawningState();
            currLevelState.EnterState();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}