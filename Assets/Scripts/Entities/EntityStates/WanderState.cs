using Core;
using Entities;
using Grid.Pathfinding;
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
        [SerializeField] private EntityState playerCloseState;
        [SerializeField] private TerrainWeights[] terrainWeights;

        [Space(15)]
        [SerializeField] private bool drawPath;
        [SerializeField] private bool drawWanderRadius;
        [SerializeField] private bool drawCheckRadius;

        private Vector3 targetLocation;
        private PathfindingBehaviour pathfindingBehaviour;

        public override void EnterState(EntityStateMachine context)
        {
            base.EnterState(context);
            GetInput();
            pathfindingBehaviour = new PathfindingBehaviour(
                targetLocation, 
                context.Speed, 
                transform, 
                context.RigidbodyContext, 
                terrainWeights.Length > 0 ? terrainWeights : context.DefaultTerrainWeights
                );
        }
        
        public override void UpdateState()
        {
            if (pathfindingBehaviour == null)
                return;

            if (pathfindingBehaviour.Path == null)
                return;

            if (pathfindingBehaviour.HasReachedEndOfPath())
            {
                GetInput();
                pathfindingBehaviour.RePath(targetLocation);
            }
            
            Move();
        }

        public override void FixedUpdateState()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, playerCheckRadius, context.EntityLayer);
            foreach (Collider collider in colliders)
            {
                if (EntityManager.IsEntity(collider.gameObject, out Entity baseClass))
                {
                    if (baseClass.EntityID == context.EntityID)
                        return;

                    if (baseClass.EntityID.Contains(context.ChaseTag))
                    {
                        context.TargetRef = collider.transform;
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

        public override void ExitState() => pathfindingBehaviour.Stop();
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
        }
    }
}
