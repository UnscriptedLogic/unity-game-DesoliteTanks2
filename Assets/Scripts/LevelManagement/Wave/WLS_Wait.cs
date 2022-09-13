using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_Wait : LevelState
    {
        public WLS_Wait(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
        }

        public void Initialize(float waitTime, bool waitsForClearedLevel)
        {

        }

        public override void EnterState()
        {
            Debug.Log("Waiting");
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }
    }
}