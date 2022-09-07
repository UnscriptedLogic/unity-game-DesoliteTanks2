using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WaveLevelFactory : LevelStateFactory
    {
        protected WLS_Countdown levelCountdown;
        protected WLS_Spawning spawning;

        public WLS_Countdown LevelCountdown => levelCountdown;
        public WLS_Spawning Spawning => spawning;

        public WaveLevelFactory(LevelStateContext context) : base(context)
        {
            levelCountdown = new WLS_Countdown(context, this);
            spawning = new WLS_Spawning(context, this);
        }
    }
}