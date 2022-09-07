using CameraManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LevelStateContext : MonoBehaviour
    {
        public LevelState currLevelState;
        private LevelStateFactory levelStateFactory;

        [Header("Level State Context")]
        [SerializeField] protected Transform playerStart;
        [SerializeField] protected GameObject playerPrefab;
        [SerializeField] protected CameraController cameraController;

        protected Transform player;

        public Transform PlayerStart => playerStart;
        public GameObject PlayerPrefab => playerPrefab;
        public CameraController CameraController => cameraController;
        public Transform Player { get => player; set { player = value; } }

        protected void StartStateMachine(LevelState startState)
        {
            currLevelState = startState;
            currLevelState.EnterState();
        }

        protected virtual void Update()
        {
            currLevelState.UpdateState();
        }

        public virtual void StateAfterSetUp()
        {
            //currLevelState = newState;
            //currLevelState.EnterState();
        }
    }
}