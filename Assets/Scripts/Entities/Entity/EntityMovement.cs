using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid.Pathfinding;
using System.IO;
using LevelManagement;

namespace Entities
{
    public class EntityMovement : MonoBehaviour, IMoveable
    {
        [Header("Movement Settings")]
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private EntityController entityController;

        [SerializeField] private TerrainWeights[] terrainTypes;

        [Header("Debug")]
        [SerializeField] private bool drawPath;
        [SerializeField] private Color pathColor;

        private Vector3 moveDirection;
        private Vector3 nextPos;
        private List<Vector3> path;
        private int currentWaypoint;
        private bool pathFound;
        private bool initialized;

        private PathFindingManager pathFindingManager;

        private void Awake() => entityController.OnTargetSet += Initialize;
        private void Start() => pathFindingManager = PathFindingManager.instance;

        private void Initialize()
        {
            path = null;
            pathFound = false;
            initialized = true;
            currentWaypoint = 0;
            nextPos = Vector3.zero;
            moveDirection = Vector3.zero;
            GetPath();
        }

        private void OnPathFound(List<Vector3> path, bool success)
        {
            pathFound = success;
            if (success)
            {
                this.path = path;
                currentWaypoint = 0;
            }
        }

        private void Update()
        {
            if (!initialized)
                return;

            if (pathFound)
            {
                if (Vector3.Distance(transform.position, nextPos) < 0.1f)
                {

                    if (currentWaypoint > 0)
                    {
                        GetPath();
                    }

                    currentWaypoint++;
                }

                if (currentWaypoint >= path.Count)
                {
                    moveDirection = Vector3.zero;
                    GetPath();
                    return;
                }

                nextPos = new Vector3(path[currentWaypoint].x, transform.position.y, path[currentWaypoint].z);
                moveDirection = (nextPos - transform.position).normalized;
            } else
            {
                moveDirection = Vector3.zero;
            }

            Move();
        }

        private void GetPath()
        {
            Transform target = entityController.GetTarget(out bool isValid);

            if (isValid)
            {
                PathFindingRequester.RequestPath(transform.position, target.position, OnPathFound, terrainTypes);
            }
        }

        public void GetInput()
        {
            //Empty Function
        }

        public void Move()
        {
            Vector3 moveVelocity = moveDirection * speed;
            rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

            if (moveDirection.magnitude > 0)
            {
                transform.forward = moveDirection;
            }
        }

        private void OnDisable()
        {
            path = null;
            pathFound = false;
            initialized = false;
            currentWaypoint = 0;
            nextPos = Vector3.zero;
            moveDirection = Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        Gizmos.color = pathColor;
                        Gizmos.DrawSphere(path[i], 0.15f);

                        if (i < path.Count - 1)
                        {
                            Gizmos.DrawLine(path[i], path[i + 1]);
                        }
                    }
                }
            }
        }
    }
}