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
        [SerializeField] private Vector2 attackIntervalOverride;
        [SerializeField] private float checkInterval;
        [SerializeField] private float giveupDistance;
        [SerializeField] private EntityState onGiveUpState;
        [SerializeField] private TerrainWeights[] terrainWeights;
        
        [SerializeField] private bool drawGiveUpDistance;
        [SerializeField] private bool drawPath;
        private bool drawStateGizmos;

        private PathfindingBehaviour pathFinder;
        private Vector2 defaultAttackInterval;
        private float _checkInterval;
        private bool shouldRepath;

        public override void EnterState(EntityStateMachine context)
        {
            base.EnterState(context);
            drawStateGizmos = true;
            pathFinder = new PathfindingBehaviour(
                context.TargetLocation,
                context.Speed,
                transform,
                context.RigidbodyContext,
                terrainWeights.Length > 0 ? terrainWeights : context.DefaultTerrainWeights
                );

            defaultAttackInterval = context.AttackScript.AttackInterval;
            context.AttackScript.AttackInterval = attackIntervalOverride;
            pathFinder.ReachedNextWaypoint += OnNextWaypoint;
            _checkInterval = checkInterval;
            initialized = true;
        }

        private void OnNextWaypoint()
        {
            if (pathFinder.HasReachedEndOfPath() || shouldRepath)
            {
                pathFinder.RePath(context.Target.position);
                shouldRepath = false;
            }
        }

        public override void UpdateState()
        {
            if (!initialized) return;

            if (_checkInterval <= 0f)
            {
                _checkInterval = checkInterval;
                shouldRepath = context.IsTargetAlive;

                if (ShouldGiveUp())
                {
                    context.SetCurrentState(onGiveUpState);
                    return;
                }
            }
            else
            {
                _checkInterval -= Time.deltaTime;
            }

            Move();
        }

        private bool ShouldGiveUp()
        {
            return !context.IsTargetAlive || Vector3.Distance(context.Target.position, transform.position) >= giveupDistance;
        }

        public void GetInput() { } 
        public void Move() => pathFinder.UpdateMove();
        
        public override void ExitState()
        {
            pathFinder.Stop();
            drawStateGizmos = false;
            initialized = false;
            context.AttackScript.AttackInterval = defaultAttackInterval;
        }

        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                if (pathFinder == null)
                    return;

                if (pathFinder.Path != null)
                {
                    for (int i = 0; i < pathFinder.Path.Count; i++)
                    {
                        Gizmos.color = context.PathGizmoColor;
                        Gizmos.DrawSphere(pathFinder.Path[i], 0.15f);

                        if (i < pathFinder.Path.Count - 1)
                        {
                            Gizmos.DrawLine(pathFinder.Path[i], pathFinder.Path[i + 1]);
                        }
                    }
                }
            }

            if (drawGiveUpDistance)
            {
                Gizmos.DrawWireSphere(transform.position, giveupDistance);
            }

            if (drawStateGizmos)
            {
                Gizmos.color = Color.red; 
                Gizmos.DrawWireCube(transform.position, Vector3.one);
            }
        }
    }
}