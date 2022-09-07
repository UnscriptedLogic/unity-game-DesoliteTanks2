using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LevelStateFactory
    {
        protected LevelState levelState;
        protected LS_LevelSetUp levelSetUp;

        public LevelState LevelState => LevelState;
        public LS_LevelSetUp LevelSetUp => levelSetUp;

        public LevelStateFactory(LevelStateContext context)
        {
            levelState = new LevelState(context, this);
            levelSetUp = new LS_LevelSetUp(context, this);
        }
    }
}