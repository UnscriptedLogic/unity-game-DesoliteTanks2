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

        public static EntityManager emInstance;
        public List<EntityTeam> entityTeams = new List<EntityTeam>();

        [SerializeField] protected EntityPoolManager entityPoolManager;
        [SerializeField] protected PathFindingManager pathfindingManager;

        [SerializeField] protected bool drawBoxes;
        protected List<GameObject> liveEntities = new List<GameObject>();
        protected List<GameObject> deadEntities = new List<GameObject>();

        public Action<GameObject> OnEntityCreated;
        public Action<Entity> OnEntityDeath;

        protected virtual void Awake()    
        {
            emInstance = this;
        }
        
        public GameObject CreateEntity(GameObject entityPrefab, string teamName)
        {
            GameObject newEntity = entityPoolManager.PullFromPool(entityPrefab, entity => { });

            GetTeam(teamName).entities.Add(newEntity);
            newEntity.GetComponent<Entity>().Team = teamName;

            liveEntities.Add(newEntity);
            if (deadEntities.Contains(newEntity))
            {
                deadEntities.Remove(newEntity);
            }

            newEntity.transform.SetParent(null);
            OnEntityCreated?.Invoke(newEntity);
            return newEntity;
        }

        public GameObject CreateEntity(GameObject entityPrefab, Action<GameObject> createdEntity)
        {
            GameObject newEntity = entityPoolManager.PullFromPool(entityPrefab, entity => { });
            createdEntity(newEntity);

            liveEntities.Add(newEntity);
            if (deadEntities.Contains(newEntity))
            {
                deadEntities.Remove(newEntity);
            }

            newEntity.transform.SetParent(null);
            OnEntityCreated?.Invoke(newEntity);
            return newEntity;
        }

        public void RemoveEntity(GameObject obj, float delay = 0f) => StartCoroutine(DoRemove(obj, delay));
        private IEnumerator DoRemove(GameObject toRemove, float delay)
        {
            yield return new WaitForSeconds(delay);
            EntityPoolManager.entityPoolInstance.PushToPool(toRemove);

            string teamName = toRemove.GetComponent<Entity>().Team;
            for (int i = 0; i < entityTeams.Count; i++)
            {
                if (entityTeams[i].teamName == teamName)
                {
                    entityTeams[i].entities.Remove(toRemove);
                }
            }

            deadEntities.Add(toRemove);
            if (liveEntities.Contains(toRemove))
            {
                liveEntities.Remove(toRemove);
            }

            OnEntityDeath?.Invoke(toRemove.GetComponent<Entity>());
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
            }

            return entityTeam;
        }

        public GameObject GetLiveEntityByID(string id) => liveEntities.Find(x => x.GetComponent<Entity>().EntityID.Contains(id));
        public GameObject GetDeadEntityByID(string id) => deadEntities.Find(x => x.GetComponent<Entity>().EntityID.Contains(id));
        public Entity GetEntityWithID(string id)
        {
            GameObject entity = GetLiveEntityByID(id);
            if (entity == null)
            {
                entity = GetDeadEntityByID(id);
            }

            return entity.GetComponent<Entity>();
        }
        public static bool IsEntity(GameObject gameObject, out Entity entity) => gameObject.TryGetComponent(out entity);
        public static bool IsEntity(Transform gameObject, out Entity entity) => gameObject.TryGetComponent(out entity);
        
        protected void OnDrawGizmos()
        {
            if (drawBoxes)
            {
                foreach (GameObject entities in liveEntities)
                {
                    Gizmos.color = GetTeam(entities.GetComponent<Entity>().Team).teamColor;
                    Gizmos.DrawWireCube(pathfindingManager.NodeFromWorldPoint(entities.transform.position).worldPos, Vector3.one * 0.9f);
                }
            }
        }
    }
}