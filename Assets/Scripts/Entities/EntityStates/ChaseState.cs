using Core;
using Grid.Pathfinding;
using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities
{
    public class ChaseState : EntityState, IMoveable
    {
        [Header("Chase State Extension")]
        [SerializeField] private float giveupDistance;
        [SerializeField] private TerrainWeights[] terrainWeights;
        [SerializeField] private bool drawGiveUpDistance;
        [SerializeField] private EntityState onGiveUpState;

        private PathfindingBehaviour pathfindingBehaviour;
        [SerializeField] private bool drawPath;

        public override void EnterState(EntityStateMachine context)
        {
            base.EnterState(context);
            pathfindingBehaviour = new PathfindingBehaviour(
                context.TargetLocation,
                context.Speed,
                transform,
                context.RigidbodyContext,
                terrainWeights.Length > 0 ? terrainWeights : context.DefaultTerrainWeights
                );

            pathfindingBehaviour.ReachedNextWaypoint += OnNextWaypoint;
        }

        private void OnNextWaypoint()
        {
            if (!context.TargetRef.gameObject.activeInHierarchy)
            {
                context.SetCurrentState(onGiveUpState);
            }

            if (Vector3.Distance(context.TargetRef.position, transform.position) >= giveupDistance)
            {
                context.SetCurrentState(onGiveUpState);
            }

            GetInput();
            context.TargetLocation = context.TargetRef.position;
            //pathfindingBehaviour.RePath(context.TargetLocation);
        }

        public override void UpdateState()
        {
            if (pathfindingBehaviour == null)
                return;

            if (pathfindingBehaviour.Path == null)
                return;

            Move();
        }

        public void GetInput() 
        {
            if (context.TargetRef == null)
            {
                Transform potentialTarget = null;
                float closestDist = Mathf.Infinity;
                Collider[] colliders = Physics.OverlapSphere(transform.position, giveupDistance, context.EntityLayer);
                foreach (Collider collider in colliders)
                {
                    Entity baseClass = collider.GetComponent<Entity>();
                    if (baseClass != null)
                    {
                        if (baseClass.EntityID == context.EntityID)
                            return;

                        if (baseClass.EntityID.Contains(context.ChaseTag))
                        {
                            float dist = Vector3.Distance(transform.position, baseClass.transform.position);
                            if (dist < closestDist)
                            {
                                closestDist = dist;
                                potentialTarget = collider.transform;
                            }
                        }
                    }
                }

                if (potentialTarget != null)
                {
                    context.TargetRef = potentialTarget;
                    context.TargetLocation = potentialTarget.position;
                    return;
                }
            }
        }

        public void Move() => pathfindingBehaviour.UpdateMove();
        public override void ExitState() => pathfindingBehaviour.Stop();

        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                if (pathfindingBehaviour == null)
                    return;

                if (pathfindingBehaviour.Path != null)
                {
                    for (int i = 0; i < pathfindingBehaviour.Path.Count; i++)
                    {
                        Gizmos.color = context.PathGizmoColor;
                        Gizmos.DrawSphere(pathfindingBehaviour.Path[i], 0.15f);

                        if (i < pathfindingBehaviour.Path.Count - 1)
                        {
                            Gizmos.DrawLine(pathfindingBehaviour.Path[i], pathfindingBehaviour.Path[i + 1]);
                        }
                    }
                }
            }

            if (drawGiveUpDistance)
            {
                Gizmos.DrawWireSphere(transform.position, giveupDistance);
            }
        }
    }
}