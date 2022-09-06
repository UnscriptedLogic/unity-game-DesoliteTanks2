using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LevelStateFactory
    {
        protected LevelState levelState;
        protected LevelSetUp levelSetUp;

        public LevelState LevelState => LevelState;
        public LevelSetUp LevelSetUp => levelSetUp;

        public LevelStateFactory(LevelStateContext context)
        {
            levelState = new LevelState(context, this);
            levelSetUp = new LevelSetUp(context, this);
        }
    }
}