using Grid.Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private bool drawGrid;
        private Vector3 center;

        public Vector2Int GridSize => gridSize;

        private void OnDrawGizmos()
        {
            if (!drawGrid)
                return;

            center = Vector2.zero - new Vector2(gridSize.x / 2, gridSize.y / 2);

            Gizmos.color = Color.white;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int z = 0; z < gridSize.y; z++)
                {
                    Gizmos.DrawWireCube(new Vector3(center.x + x, transform.position.y, center.y + z), Vector3.one);
                }
            }
        }
    }
}