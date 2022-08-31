using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid.Pathfinding;
using System.IO;

namespace Entities
{
    public class EntityMovement : MonoBehaviour, IMoveable
    {
        [SerializeField] private float speed;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private EntityController entityController;
        [SerializeField] private bool drawPath;

        private Transform target;
        private Vector3 moveDirection;
        private Vector3 nextPos;
        private List<Vector3> path;
        private int currentWaypoint;
        private bool pathFound;
        private bool initialized;

        [SerializeField] private int repathAfterNodes = 2;
        [SerializeField] private float repathInterval = 1f;
        private float _repathInterval;

        private PathFindingManager pathFindingManager;
        private PFNode prevPathfindCallNode;

        private void Awake()
        {
            entityController.OnTargetSet += Initialize;
        }

        private void OnEnable()
        {
            pathFindingManager = PathFindingManager.instance;
        }
        
        private void Initialize()
        {
            initialized = true;
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

                if (Vector3.Distance(prevPathfindCallNode.worldPos, transform.position) > repathAfterNodes)
                {
                    GetPath();
                }
            } else
            {
                moveDirection = Vector3.zero;
                if (_repathInterval <= 0f)
                {
                    GetPath();
                    _repathInterval = repathInterval;
                }
            }

            if (_repathInterval >= 0f)
            {
                _repathInterval -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void GetPath()
        {
            Transform target = entityController.GetTarget(out bool isValid);

            if (isValid)
            {
                PathFindingRequester.RequestPath(transform.position, target.position, OnPathFound);
                prevPathfindCallNode = PathFindingManager.instance.NodeFromWorldPoint(transform.position);
            }

        }

        public void GetInput()
        {
            //Empty Function
        }

        public void Move()
        {
            if (moveDirection.magnitude > 0f)
            {
                rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
                transform.forward = moveDirection;
            }
        }

        private void OnDrawGizmos()
        {
            if (drawPath)
            {
                if (path != null)
                {
                    for (int i = 0; i < path.Count; i++)
                    {
                        Gizmos.color = Color.green;
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