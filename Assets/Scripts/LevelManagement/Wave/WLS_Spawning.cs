using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_Spawning : LevelState
    {
        private WaveLevelContext waveLevelContext;
        private WaveLevelFactory waveLevelFactory;

        public WLS_Spawning(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            waveLevelContext = (WaveLevelContext)context;
            waveLevelFactory = (WaveLevelFactory)factory;
        }

        public override void EnterState()
        {

        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {

        }
    }
}