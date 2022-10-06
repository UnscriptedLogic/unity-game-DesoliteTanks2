using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class EntityState : MonoBehaviour
    {
        protected EntityStateMachine context;
        protected bool initialized = false;

        public virtual void EnterState(EntityStateMachine context)
        {
            this.context = context;
        }

        public virtual void UpdateState()
        {

        }

        public virtual void FixedUpdateState()
        {

        }

        public virtual void ExitState()
        {

        }
    }
}







