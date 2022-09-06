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
        [SerializeField] private List<GameObject> liveEntities = new List<GameObject>();
        [SerializeField] private List<GameObject> deadEntities = new List<GameObject>();

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
            liveEntities.Add(obj);
            if (deadEntities.Contains(obj))
            {
                deadEntities.Remove(obj);
            }
        }

        private void RemoveEntity(GameObject obj)
        {
            deadEntities.Add(obj);
            if (liveEntities.Contains(obj))
            {
                liveEntities.Remove(obj);
            }
        }

        public GameObject GetLiveEntityByID(string id)
        {
            return liveEntities.Find(x => x.GetComponent<BaseManagerClass>().EntityID == id);
        }

        public GameObject GetDeadEntityByID(string id)
        {
            return deadEntities.Find(x => x.GetComponent<BaseManagerClass>().EntityID == id);
        }

        private void OnDrawGizmos()
        {
            if (drawBoxes)
            {
                foreach (GameObject entities in liveEntities)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireCube(pathfindingManager.NodeFromWorldPoint(entities.transform.position).worldPos, Vector3.one * 0.9f);
                }
            }
        }
    }
}