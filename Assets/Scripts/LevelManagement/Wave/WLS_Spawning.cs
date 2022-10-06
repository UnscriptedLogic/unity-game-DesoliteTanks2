using Grid.Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_Spawning : LevelState
    {
        private WaveLevelContext wlContext;
        private WaveLevelFactory waveLevelFactory;

        private int entityIndex;
        private int spawnedEntities;
        private WL_SpawnList listToSpawn;
        private WL_Spawn setToSpawn;
        private float _entityInterval;
        private float _setInterval;

        public WLS_Spawning(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            wlContext = (WaveLevelContext)context;
            waveLevelFactory = (WaveLevelFactory)factory;
        }

        public override void EnterState()
        {
            if (wlContext.WL_SpawnList.Count <= 0)
            {
                Debug.Log("There was nothing to spawn");
                SwitchState(waveLevelFactory.Waiting(0f, false));
                return;
            }

            _entityInterval = 0;
            _setInterval = 0;

            entityIndex = 0;
            spawnedEntities = 0;
            wlContext.WaveIndex++;

            if (wlContext.WaveIndex >= wlContext.WL_SpawnList.Count)
            {
                SwitchState(waveLevelFactory.EndGame(true));
                return;
            }

            listToSpawn = wlContext.WL_SpawnList[wlContext.WaveIndex];
            setToSpawn = listToSpawn.spawnList[entityIndex];
        }

        public override void UpdateState()
        {
            if (_setInterval <= 0f)
            {
                if (_entityInterval <= 0f)
                {
                    SpawnEntity(setToSpawn.entity);
                    _entityInterval = setToSpawn.entitySpawnInterval;
                    spawnedEntities++;

                    if (spawnedEntities >= setToSpawn.amountToSpawn)
                    {
                        entityIndex++;
                        if (entityIndex >= listToSpawn.spawnList.Count)
                        {
                            ExitState();
                        }
                        else
                        {
                            spawnedEntities = 0;
                            _setInterval = setToSpawn.completeDelay;
                            setToSpawn = listToSpawn.spawnList[entityIndex];
                        }
                    }
                }
                else
                {
                    _entityInterval -= Time.deltaTime;
                }
            } else
            {
                _setInterval -= Time.deltaTime;
            }
        }

        public override void ExitState()
        {
            SwitchState(waveLevelFactory.Waiting(listToSpawn.entityGroupSpawnInterval, listToSpawn.WaitsForClearedLevel));
        }

        private void SpawnEntity(GameObject entity)
        {
            int maxTries = 50;
            while (maxTries > 0)
            {
                Vector3 spawnLocation = RandomizeSpawnLocation();
                spawnLocation.y = wlContext.Center.y;
                if (wlContext.SpawnManager.SpawnEntity(entity, 1f, spawnLocation))
                {
                    break;
                }

                maxTries--;
            }
        }

        private Vector3 RandomizeSpawnLocation()
        {
            float randomX = UnityEngine.Random.Range(-wlContext.SpawnArea.x / 2, wlContext.SpawnArea.x / 2);
            float randomZ = UnityEngine.Random.Range(-wlContext.SpawnArea.y / 2, wlContext.SpawnArea.y / 2);

            return PathFindingManager.instance.NodeFromWorldPoint(new Vector3(wlContext.Center.x + randomX, 0f, wlContext.Center.z + randomZ)).worldPos;
        }
    }
}