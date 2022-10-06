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
        [SerializeField] private SpawningManager spawningManager;
        [SerializeField] private List<GameObject> entitiesToSpawn = new List<GameObject>();
        [SerializeField] private float spawnDelay;

        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private Button playButton;
        private MainMenuFactory mainMenuFactory;

        public SpawningManager SpawningManager => spawningManager;
        public List<GameObject> EntitiesToSpawn => entitiesToSpawn;
        public float SpawnDelay => spawnDelay;
        public Vector2 SpawnArea => spawningArea;
        public Vector3 Center => center;

        private void Start()
        {
            mainMenuFactory = new MainMenuFactory(this);
            StartStateMachine(mainMenuFactory.LevelSetUp);

            playButton.onClick.AddListener(() =>
            {
                sceneChanger.ChangeScene(SceneChanger.GAME_SCENE);
            });
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