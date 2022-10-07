namespace LevelManagement
{
    public class WLS_SetUp : LS_LevelSetUp
    {
        private WaveLevelContext wlContext;

        public WLS_SetUp(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            wlContext = (WaveLevelContext)context;
        }

        public override void EnterState()
        {
            if (GameManager.hasLevelDetails)
            {
                WLDetailsSO levelDetails = (WLDetailsSO)GameManager.levelDetails;
                wlContext.LevelDetails = levelDetails;
                wlContext.WL_SpawnList = levelDetails.SpawnList;
                LoadMap(levelDetails.MapName);
            }
            else
            {
                LoadMap(GameManager.FALLBACK_MAP);
            }

            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}