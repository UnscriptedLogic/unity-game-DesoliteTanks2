using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid.Pathfinding
{
    public class PathFindingManager : MonoBehaviour
    {
        [SerializeField] private GridManager gridManager;
        [SerializeField] private LayerMask unwalkableMask;
        [SerializeField] private float nodeRadius;
        [SerializeField] private PathFindingRequester pathRequester;

        [SerializeField] private bool drawObstacles;
        [SerializeField] private bool drawPath;
        
        private PFNode[,] pfNodes;

        private float nodeDiameter;
        private int gridSizeX, gridSizeY;

        [SerializeField] public static PathFindingManager instance;

        //Temp
        private List<Vector3> path;

        private void Awake()
        {
            instance = this;
            nodeDiameter = nodeRadius * 2;
            gridSizeX = Mathf.RoundToInt(gridManager.GridSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridManager.GridSize.y / nodeDiameter);

            CreateGrid();
        }

        private void Start()
        {
            BlockUpdater.blockUpdaterInstance.OnBlockDestroyed += ReCalculateObstacleNode;
            BlockUpdater.blockUpdaterInstance.OnBlockCreated += ReCalculateObstacleNode;
        }

        private void CreateGrid()
        {
            pfNodes = new PFNode[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = gridManager.transform.position - Vector3.right * gridManager.GridSize.x / 2 - Vector3.forward * gridManager.GridSize.y / 2;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));

                    pfNodes[x, y] = new PFNode(walkable, worldPoint, x, y);
                }
            }
        }

        public void ReCalculateObstacleNode(BlockDetails blockDetails)
        {
            NodeFromWorldPoint(blockDetails.position).walkable = !Physics.CheckSphere(blockDetails.position, nodeRadius, unwalkableMask);
        }

        public PFNode NodeFromWorldPoint(Vector3 worldPos)
        {
            float percentX = worldPos.x / gridManager.GridSize.x + 0.5f;
            float percentY = worldPos.z / gridManager.GridSize.y + 0.5f;
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
            return pfNodes[x, y];
        }

        public void StartFindPath(Vector3 startPosition, Vector3 targetPosition, TerrainType[] terrainTypes = null)
        {
            StartCoroutine(FindPath(startPosition, targetPosition, terrainTypes));
        }

        private IEnumerator FindPath(Vector3 start, Vector3 end, TerrainType[] terrainTypes = null)
        {
            LayerMask allWalkableMasks = new LayerMask();
            Dictionary<int, int> walkableDict = new Dictionary<int, int>();
            if (terrainTypes != null)
            {
                foreach (TerrainType terrainType in terrainTypes)
                {
                    allWalkableMasks |= terrainType.terrainMask.value;
                    walkableDict.Add((int)MathF.Log(terrainType.terrainMask.value, 2), terrainType.terrainPenalty);
                }
            }

            bool pathSuccess = false;

            PFNode startNode = NodeFromWorldPoint(start);
            PFNode targetNode = NodeFromWorldPoint(end);

            if (startNode.walkable && targetNode.walkable)
            {
                Heap<PFNode> openSet = new Heap<PFNode>(gridSizeX * gridSizeY);
                HashSet<PFNode> closedSet = new HashSet<PFNode>();
                openSet.Add(startNode);
                while (openSet.Count > 0)
                {
                    PFNode currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);
                    if (currentNode == targetNode)
                    {

                        pathSuccess = true;
                        break;
                    }
                    foreach (PFNode neighbour in GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int neighbourPenalty = 0;
                        if (terrainTypes != null)
                        {
                            //Relative entity terrain type logic
                            //Ray ray = new Ray(neighbour.worldPos + Vector3.up * 10f, Vector3.down);
                            //if (Physics.Raycast(ray, out RaycastHit hit, 100f, allWalkableMasks))
                            //{
                            //    walkableDict.TryGetValue(hit.collider.gameObject.layer, out neighbourPenalty);
                            //}

                            Collider[] collider = Physics.OverlapBox(neighbour.worldPos, Vector3.one * 0.45f, Quaternion.identity, allWalkableMasks);
                            if (collider.Length > 0)
                            {
                                walkableDict.TryGetValue(collider[0].gameObject.layer, out neighbourPenalty);
                            }
                        }

                        int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbourPenalty;
                        if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbour;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;
                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                        }
                    }
                }
            }

            yield return null;

            if (pathSuccess)
            {
                RetracePath(startNode, targetNode);
            }

            pathRequester.OnPathFound(path, pathSuccess);
        }

        private IEnumerable<PFNode> GetNeighbours(PFNode currentNode)
        {
            List<PFNode> neighbours = new List<PFNode>();

            if (currentNode.gridX - 1 >= 0)
            {
                neighbours.Add(pfNodes[currentNode.gridX - 1, currentNode.gridY]);
            }

            if (currentNode.gridY - 1 >= 0)
            {
                neighbours.Add(pfNodes[currentNode.gridX, currentNode.gridY - 1]);
            }

            if (currentNode.gridX + 1 < gridSizeX)
            {
                neighbours.Add(pfNodes[currentNode.gridX + 1, currentNode.gridY]);
            }

            if (currentNode.gridY + 1 < gridSizeY)
            {
                neighbours.Add(pfNodes[currentNode.gridX, currentNode.gridY + 1]);
            }

            return neighbours;
        }

        public int GetDistance(PFNode currentNode, PFNode neighbour)
        {
            int dstX = Mathf.Abs(currentNode.gridX - neighbour.gridX);
            int dstY = Mathf.Abs(currentNode.gridY - neighbour.gridY);
            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }

        private List<Vector3> RetracePath(PFNode startNode, PFNode targetNode)
        {
            path = new List<Vector3>();
            PFNode currentNode = targetNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode.worldPos);
                currentNode = currentNode.parent;
            }

            //Simplifies the path by removing nodes in between turning points
            for (int i = 0; i < path.Count; i++)
            {
                if (i > 0 && i < path.Count - 1)
                {
                    Vector3 prevDir = path[i] - path[i - 1];
                    Vector3 nextDir = path[i + 1] - path[i];

                    if (prevDir.normalized == nextDir.normalized)
                    {
                        path.RemoveAt(i);
                        i--;
                    }
                }
            }
            
            path.Reverse();
            return path;
        }

        private void OnDrawGizmos()
        {
            if (drawObstacles)
            {
                if (pfNodes != null)
                {
                    foreach (PFNode pFNode in pfNodes)
                    {
                        if (!pFNode.walkable)
                        {
                            Gizmos.color = Color.red;
                            Gizmos.DrawWireCube(pFNode.worldPos, Vector3.one * (nodeDiameter - .1f));
                        }
                    }
                }
            }
        }
    }

    public class PFNode : IHeapItem<PFNode>
    {
        public bool walkable;
        public Vector3 worldPos;
        public int fCost => gCost + hCost;

        public int heapIndex;
        public int HeapIndex { get => heapIndex; set => heapIndex = value; }

        public int hCost;
        public int gCost;
        public PFNode parent;
        public int gridX;
        public int gridY;

        public PFNode(bool walkable, Vector3 worldPos, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPos = worldPos;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int CompareTo(PFNode other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(other.hCost);
            }
            return -compare;
        }
    }

    [Serializable]
    public class TerrainType
    {
        public LayerMask terrainMask;
        public int terrainPenalty;
    }
}