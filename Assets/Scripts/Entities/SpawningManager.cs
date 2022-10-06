using Core;
using Grid.Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Entities.Spawning
{
    public class SpawningManager : MonoBehaviour
    {
        [SerializeField] private string team = "turned";
        [SerializeField] private LayerMask obstacleLayer;

        public string Team => team;

        public bool SpawnEntity(GameObject entityPrefab, float size, Vector3 spawnLocation)
        {
            if (IsSpawnValid(spawnLocation, size))
            {
                GameObject newEntity = EntityManager.emInstance.CreateEntity(entityPrefab, createdEntity => 
                {
                    Entity baseClass = createdEntity.GetComponent<Entity>();
                    if (baseClass.EntityID.Contains("tank"))
                    {
                        EntityManager.emInstance.GetTeam(team).entities.Add(createdEntity);
                        baseClass.Team = team;
                    }
                    else
                    {
                        EntityManager.emInstance.GetTeam(baseClass.Team).entities.Add(createdEntity);
                    }
                });

                newEntity.transform.SetPositionAndRotation(spawnLocation, Quaternion.Euler(0f, 180f, 0f));
                newEntity.SetActive(true);
                return true;
            }

            return false;
        }

        private bool IsSpawnValid(Vector3 posToCheck, float checkRadius)
        {
            return !Physics.CheckSphere(posToCheck, checkRadius, obstacleLayer);
        }
    }
}