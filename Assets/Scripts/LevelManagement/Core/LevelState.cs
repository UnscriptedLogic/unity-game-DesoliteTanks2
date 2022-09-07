using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LevelState
    {
        protected LevelStateContext context;
        protected LevelStateFactory factory;

        public LevelState(LevelStateContext context, LevelStateFactory factory)
        {
            this.context = context;
            this.factory = factory;
        }

        public virtual void EnterState()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void ExitState()
        {

        }

        public void SwitchState(LevelState newLevelState)
        {
            context.currLevelState = newLevelState;
            context.currLevelState.EnterState();
        }
    }
}