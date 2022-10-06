using UnityEngine;

namespace LevelManagement
{
    public class MMS_Waiting : LevelState
    {
        private MainMenuContext mmContext;
        private MainMenuFactory mmFactory;
        private float waitTime;

        public MMS_Waiting(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            mmContext = (MainMenuContext)context;
            mmFactory = (MainMenuFactory)factory;
        }

        public override void EnterState()
        {
            waitTime = mmContext.SpawnDelay;
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            if (waitTime <= 0f)
            {
                SwitchState(mmFactory.SpawningState());
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
