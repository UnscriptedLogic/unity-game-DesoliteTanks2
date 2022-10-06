using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WaveLevelFactory : LevelStateFactory
    {
        protected WLS_Countdown levelCountdown;
        protected WLS_Spawning spawning;
        protected WLS_Wait waiting;
        protected WLS_EndGame endGame;

        public WaveLevelFactory(LevelStateContext context) : base(context)
        {
            levelCountdown = new WLS_Countdown(context, this);
            spawning = new WLS_Spawning(context, this);
            waiting = new WLS_Wait(context, this);
            endGame = new WLS_EndGame(context, this);
        }

        public LevelState CountDown() => levelCountdown;
        public LevelState Spawning() => spawning;

        public LevelState Waiting(float waitingTime, bool waitsForClearedLevel)
        {
            waiting.Initialize(waitingTime, waitsForClearedLevel);
            return waiting;
        }

        public LevelState EndGame(bool gameWon)
        {
            endGame.Initialize(gameWon);
            return endGame;
        }
    }
}




