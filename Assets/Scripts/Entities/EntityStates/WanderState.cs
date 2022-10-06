using Core;
using Entities;
using Grid.Pathfinding;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Entities
{
    public class WanderState : EntityState, IMoveable
    {
        [Header("Wander State Extension")]
        [SerializeField] private float wanderRadius;
        [SerializeField] private float playerCheckRadius;
        [SerializeField] private float nextStateDelay;
        [SerializeField] private float rewanderInterval;
        [SerializeField] private EntityState playerCloseState;
        [SerializeField] private TerrainWeights[] terrainWeights;

        private float _nextStateDelay;
        private float _rewanderInterval;
        private bool canSwitchState;
        private bool shouldRepath;

        [Space(15)]
        [SerializeField] private bool drawPath;
        [SerializeField] private bool drawWanderRadius;
        [SerializeField] private bool drawCheckRadius;
        private bool drawStateGizmo;

        private Vector3 targetLocation;
        private PathfindingBehaviour pathfindingBehaviour;

        public override void EnterState(EntityStateMachine context)
        {
            drawStateGizmo = true;
            base.EnterState(context);
            GetInput();
            pathfindingBehaviour = new PathfindingBehaviour(
                targetLocation, 
                context.Speed, 
                transform, 
                context.RigidbodyContext, 
                terrainWeights.Length > 0 ? terrainWeights : context.DefaultTerrainWeights
                );

            pathfindingBehaviour.ReachedNextWaypoint += OnNextWaypoint;

            _nextStateDelay = nextStateDelay;
            canSwitchState = false;
            shouldRepath = false;
            initialized = true;
        }

        private void OnNextWaypoint()
        {
            canSwitchState = _nextStateDelay <= 0f;

            if (shouldRepath)
            {
                GetInput();
                pathfindingBehaviour.RePath(targetLocation);
                _rewanderInterval = rewanderInterval;
            }
        }

        public override void UpdateState()
        {
            if (!initialized) return;

            if (_rewanderInterval <= 0f)
            {
                shouldRepath = true;
            }

            _rewanderInterval -= Time.deltaTime;
            _nextStateDelay -= Time.deltaTime;

            Move();
        }

        public override void FixedUpdateState()
        {
            if (!canSwitchState) return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, context.EntityLayer);
            foreach (Collider collider in colliders)
            {
                if (EntityManager.IsEntity(collider.gameObject, out Entity baseClass))
                {
                    if (baseClass.EntityID == context.EntityID)
                        continue;

                    if (baseClass.EntityID.Contains(context.ChaseTag))
                    {
                        context.Target = collider.transform;
                        context.TargetLocation = collider.transform.position;
                        context.SetCurrentState(playerCloseState);
                        return;
                    }
                }
            }
        }

        public void GetInput()
        {
            float randomX = transform.position.x + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
            float randomZ = transform.position.z + UnityEngine.Random.Range(-wanderRadius, wanderRadius);
            targetLocation = new Vector3(randomX, transform.position.y, randomZ);
        }

        public override void ExitState()
        {
            pathfindingBehaviour.Stop();
            drawStateGizmo = false;
            initialized = false;
        }
        
        public void Move() => pathfindingBehaviour.UpdateMove();

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

            if (drawWanderRadius)
            {
                Gizmos.DrawWireSphere(transform.position, wanderRadius);
            }

            if (drawCheckRadius)
            {
                Gizmos.DrawWireSphere(transform.position, playerCheckRadius);
            }

            if (drawStateGizmo)
            {
                Gizmos.color = Color.green; 
                Gizmos.DrawWireCube(transform.position, Vector3.one);
            }
        }
    }
}
