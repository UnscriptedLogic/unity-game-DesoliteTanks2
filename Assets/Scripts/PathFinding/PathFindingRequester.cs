using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid.Pathfinding
{
    public class PathFindingRequester : MonoBehaviour
    {
        [SerializeField] private PathFindingManager pathFindingManager;
        private Queue<PathFindingRequest> requestQueue = new Queue<PathFindingRequest>();
        private PathFindingRequest currentRequest;

        private bool isProcessing;
        
        public static PathFindingRequester Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public static void RequestPath(Vector3 startPosition, Vector3 targetPosition, Action<List<Vector3>, bool> callback, TerrainType[] terrainTypes = null)
        {
            PathFindingRequest request = new PathFindingRequest(startPosition, targetPosition, callback);
            Instance.requestQueue.Enqueue(request);
            Instance.TryProcessNext(terrainTypes);
        }

        private void TryProcessNext(TerrainType[] terrainTypes = null)
        {
            if (isProcessing || requestQueue.Count == 0)
                return;

            currentRequest = requestQueue.Dequeue();
            isProcessing = true;
            pathFindingManager.StartFindPath(currentRequest.startPosition, currentRequest.targetPosition, terrainTypes);
        }

        public void OnPathFound(List<Vector3> path, bool success)
        {
            isProcessing = false;
            currentRequest.callback(path, success);
            TryProcessNext();
        }

        struct PathFindingRequest
        {
            public Vector3 startPosition;
            public Vector3 targetPosition;
            public Action<List<Vector3>, bool> callback;
            public PathFindingRequest(Vector3 startPosition, Vector3 targetPosition, Action<List<Vector3>, bool> callback)
            {
                this.startPosition = startPosition;
                this.targetPosition = targetPosition;
                this.callback = callback;
            }
        }
    }
}