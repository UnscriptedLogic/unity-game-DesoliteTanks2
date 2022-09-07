using CameraManagement;
using Newtonsoft.Json.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

namespace LevelManagement
{
    public class WaveLevelContext : LevelStateContext
    {
        [Header("Wave Level Context")]
        [SerializeField] protected float startDelay = 5f;

        private WaveLevelFactory waveLevelFactory;

        public float StartDelay => startDelay;

        private void Start()
        {
            waveLevelFactory = new WaveLevelFactory(this);
            StartStateMachine(waveLevelFactory.LevelSetUp);
        }

        public override void StateAfterSetUp()
        {
            Debug.Log("Set up complete");
            currLevelState = waveLevelFactory.LevelCountdown;
            currLevelState.EnterState();
        }
    }
}