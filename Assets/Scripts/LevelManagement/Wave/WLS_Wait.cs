using Core;
using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_Wait : LevelState
    {
        private float waitTime;
        private bool waitsForClearedLevel;
        private WaveLevelContext wlContext;
        private WaveLevelFactory wlFactory;
        private bool isWaiting;

        public WLS_Wait(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            wlContext = (WaveLevelContext)context;
            wlFactory = (WaveLevelFactory)factory;
        }

        public void Initialize(float waitTime, bool waitsForClearedLevel)
        {
            this.waitTime = waitTime;
            this.waitsForClearedLevel = waitsForClearedLevel;
        }

        public override void EnterState()
        {
            isWaiting = false;
        }

        public override void ExitState()
        {
            
        }

        public override void UpdateState()
        {
            if (isWaiting)
            {
                return;
            }

            if (waitTime <= 0f)
            {
                if (waitsForClearedLevel)
                {
                    wlContext.EntityManager.OnEntityDeath += OnEntityDeath;
                    isWaiting = true;

                    if (wlContext.EntityManager.GetTeam(wlContext.SpawnManager.Team).entities.Count <= 0)
                    {
                        SwitchState(wlFactory.Spawning());
                    }
                }
                else
                {
                    SwitchState(wlFactory.Spawning());
                }
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        private void OnEntityDeath(Entity obj)
        {
            if (obj.GetComponent<Entity>().Team.Contains(wlContext.SpawnManager.Team))
            {
                if (wlContext.EntityManager.GetTeam(wlContext.SpawnManager.Team).entities.Count <= 0)
                {
                    SwitchState(wlFactory.Spawning());
                    wlContext.EntityManager.OnEntityDeath -= OnEntityDeath;
                }
            }
        }
    }
}