using Grid.Pathfinding;
using UnityEngine;

namespace LevelManagement
{
    public class MMS_Spawning : LevelState
    {
        private MainMenuContext mmContext;
        private MainMenuFactory mmFactory;

        public MMS_Spawning(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            mmContext = (MainMenuContext)context;
            mmFactory = (MainMenuFactory)factory;
        }

        public override void EnterState()
        {
            GameObject entityToSpawn = MathHelper.RandomFromList(mmContext.EntitiesToSpawn, out int index);
            Vector3 randomPosition = PathFindingManager.instance.NodeFromWorldPoint(MathHelper.RandomInArea(new Vector3(mmContext.Center.x + mmContext.SpawnArea.x, mmContext.Center.y, mmContext.Center.z + mmContext.SpawnArea.y))).worldPos;
            mmContext.SpawningManager.SpawnEntity(entityToSpawn, 0f, randomPosition);
            SwitchState(mmFactory.WaitingState());
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            
        }
    }
}
