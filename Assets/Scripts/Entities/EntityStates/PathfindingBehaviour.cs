using Grid.Pathfinding;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Entities
{
    public class PathfindingBehaviour
    {
        private Vector3 start;
        private Vector3 end;
        private Transform transform;
        private TerrainWeights[] weights;

        private Vector3 moveDirection;
        private Vector3 nextPos;
        private List<Vector3> path = new List<Vector3>();
        private int currentWaypoint;
        private bool pathFound;

        private float speed;
        private Rigidbody rb;

        public delegate void ReachedNextNode();
        public ReachedNextNode ReachedNextWaypoint; 

        public int WaypointIndex => currentWaypoint;
        public Vector3 NextNodePos => nextPos;
        public List<Vector3> Path => path;

        public PathfindingBehaviour(Vector3 end, float speed, Transform transform, Rigidbody rb, TerrainWeights[] weights)
        {
            this.start = transform.position;
            this.end = end;
            this.speed = speed;
            this.transform = transform;
            this.rb = rb;
            this.weights = weights;
            PathFindingRequester.RequestPath(start, end, OnPathFound, weights);
        }

        public void RePath(Vector3 end)
        {
            Stop();
            this.end = end;
            PathFindingRequester.RequestPath(transform.position, end, OnPathFound, weights);
        }

        public bool HasReachedEndOfPath()
        {
            return currentWaypoint >= path.Count;
        }

        public void UpdateMove()
        {
            if (pathFound)
            {
                //Close enought to the waypoint
                if (Vector3.Distance(transform.position, nextPos) < 0.05f)
                {
                    ReachedNextWaypoint?.Invoke();

                    //Repath after reaching the second node
                    if (currentWaypoint > 1)
                    {
                        RePath(end);
                    }

                    currentWaypoint++;
                }

                //Reached the end of the path
                if (currentWaypoint >= path.Count)
                {
                    moveDirection = Vector3.zero;
                    pathFound = false;
                    RePath(end);
                    return;
                }

                nextPos = new Vector3(path[currentWaypoint].x, transform.position.y, path[currentWaypoint].z);
                moveDirection = (nextPos - transform.position).normalized;
            }
            else
            {
                moveDirection = Vector3.zero;
            }

            Move();

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

        public void Stop()
        {
            pathFound = false;
            moveDirection = Vector3.zero;
        }
    }
}
