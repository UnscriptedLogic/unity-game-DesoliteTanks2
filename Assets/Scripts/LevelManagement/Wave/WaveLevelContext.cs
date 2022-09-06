using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WaveLevelContext : LevelStateContext
    {
        private void Start()
        {
            levelStateFactory = new LevelStateFactory(this);
            StartStateMachine(levelStateFactory.LevelSetUp);
        }
    }
}