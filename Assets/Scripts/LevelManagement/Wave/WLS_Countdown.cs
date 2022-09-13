using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_Countdown : LevelState
    {
        private float countdown;
        private WaveLevelContext wlContext;
        private WaveLevelFactory wlFactory;

        public WLS_Countdown(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            wlContext = (WaveLevelContext)context;
            wlFactory = (WaveLevelFactory)factory;
        }

        public override void EnterState()
        {
            countdown = wlContext.StartDelay;
        }

        public override void ExitState()
        {

        }
        
        public override void UpdateState()
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f)
            {
                ExitState();
                SwitchState(wlFactory.Spawning());
            }
        }
    }
}