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
        [Serializable]
        public class EntityTeam
        {
            public string teamName;
            public Color teamColor;
            public List<GameObject> entities;

            public EntityTeam(string teamName, Color teamColor)
            {
                this.teamName = teamName;
                this.teamColor = teamColor;
                entities = new List<GameObject>();
            }
        }

        public static EntityManager instance;

        public List<EntityTeam> entityTeams = new List<EntityTeam>();

        [SerializeField] private EntityPoolManager entityPoolManager;
        [SerializeField] private PathFindingManager pathfindingManager;

        [SerializeField] private bool drawBoxes;
        private List<GameObject> liveEntities = new List<GameObject>();
        private List<GameObject> deadEntities = new List<GameObject>();

        public Action<GameObject> OnEntityDeath;

        private void Awake()
        {
            instance = this;
        }
        
        public GameObject CreateEntity(GameObject entityPrefab, string teamName)
        {
            GameObject newEntity = entityPoolManager.PullFromPool(entityPrefab, entity => { });

            GetTeam(teamName).entities.Add(newEntity);
            newEntity.GetComponent<BaseManagerClass>().Team = teamName;

            liveEntities.Add(newEntity);
            if (deadEntities.Contains(newEntity))
            {
                deadEntities.Remove(newEntity);
            }

            return newEntity;
        }

        public void RemoveEntity(GameObject obj)
        {
            EntityPoolManager.entityPoolInstance.PushToPool(obj);

            string teamName = obj.GetComponent<BaseManagerClass>().Team;
            for (int i = 0; i < entityTeams.Count; i++)
            {
                if (entityTeams[i].teamName == teamName)
                {
                    entityTeams[i].entities.Remove(obj);
                }
            }

            deadEntities.Add(obj);
            if (liveEntities.Contains(obj))
            {
                liveEntities.Remove(obj);
            }

            OnEntityDeath?.Invoke(obj);
        }

        public EntityTeam GetTeam(string teamName)
        {
            EntityTeam entityTeam = null;
            for (int i = 0; i < entityTeams.Count; i++)
            {
                if (entityTeams[i].teamName == teamName)
                {
                    entityTeam = entityTeams[i];
                }
            }

            if (entityTeam == null)
            {
                entityTeam = new EntityTeam(teamName, UnityEngine.Random.ColorHSV());
                entityTeams.Add(entityTeam);
                Debug.Log("Created a new team. May not be as expected.");
            }

            return entityTeam;
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
                    Gizmos.color = GetTeam(entities.GetComponent<BaseManagerClass>().Team).teamColor;
                    Gizmos.DrawWireCube(pathfindingManager.NodeFromWorldPoint(entities.transform.position).worldPos, Vector3.one * 0.9f);
                }
            }
        }
    }
}