using Grid.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class TargetBaseState : EntityState, IMoveable, IRequiresBase
    {
        [Header("Chase Base State")]
        [SerializeField] private TerrainWeights[] overrideChaseWeights;

        private Transform baseObject;

        public override void EnterState(EntityStateMachine context)
        {


            base.EnterState(context);
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public void GetInput() { }
        public void Move()
        {

        }

        public void InitBaseObject(Transform baseObject)
        {
            this.baseObject = baseObject;
        }
    }
}