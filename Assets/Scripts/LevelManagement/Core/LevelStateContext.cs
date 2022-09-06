using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LevelStateContext : MonoBehaviour
    {
        public LevelState currLevelState;
        protected LevelStateFactory levelStateFactory;

        [Header("Level State Context")]
        [SerializeField] protected GameObject playerPrefab;
        [SerializeField] protected GameObject cameraObject;

        protected void StartStateMachine(LevelState startState)
        {
            currLevelState = startState;
            currLevelState.EnterState();
        }

        protected virtual void Update()
        {
            currLevelState.UpdateState();
        }
    }
}