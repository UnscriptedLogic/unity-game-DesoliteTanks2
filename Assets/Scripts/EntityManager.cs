using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grid.Pathfinding;
using Entities;

namespace Core
{
    public class EntityManager : MonoBehaviour
    {
        public static EntityManager instance;

        [SerializeField] private EntityPoolManager entityPoolManager;
        [SerializeField] private PathFindingManager pathfindingManager;

        [SerializeField] private bool drawBoxes;
        [SerializeField] private List<GameObject> entities = new List<GameObject>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            entityPoolManager.OnEntityPushed += RemoveEntity;
            entityPoolManager.OnEntityPulled += AddEntity;
        }

        private void AddEntity(GameObject obj)
        {
            entities.Add(obj);
        }

        private void RemoveEntity(GameObject obj)
        {
            entities.Remove(obj);
        }

        public GameObject GetEntityByID(string id)
        {
            return entities.Find(x => x.GetComponent<BaseManagerClass>().EntityID == id);
        }

        private void OnDrawGizmos()
        {
            if (drawBoxes)
            {
                foreach (GameObject entities in entities)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(pathfindingManager.NodeFromWorldPoint(entities.transform.position).worldPos, Vector3.one * 0.9f);
                }
            }
        }
    }
}