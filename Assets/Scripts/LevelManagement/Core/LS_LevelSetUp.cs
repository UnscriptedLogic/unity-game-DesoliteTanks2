using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    public class LS_LevelSetUp : LevelState
    {
        public LS_LevelSetUp(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            if (context.PlayerStart != null)
            {
                if (context.PlayerPrefab != null)
                {
                    EntityPoolManager.entityPoolInstance.PullFromPool(context.PlayerPrefab, playerObject =>
                    {
                        playerObject.transform.SetPositionAndRotation(context.PlayerStart.position, context.PlayerStart.rotation);
                        context.Player = playerObject.transform;
                        playerObject.SetActive(true);
                    });

                    context.CameraController.trackTarget = context.Player.transform;
                }
            }

            ExitState();
        }

        public override void UpdateState() { }
        public override void ExitState() => context.StateAfterSetUp();
    }
}