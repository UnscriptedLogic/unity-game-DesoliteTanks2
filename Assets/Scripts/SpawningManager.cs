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

        public bool SpawnEntity(GameObject entity, float size, Vector3 spawnLocation)
        {
            if (IsSpawnValid(spawnLocation, size))
            {
                GameObject newEntity = EntityManager.instance.CreateEntity(entity, team);
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