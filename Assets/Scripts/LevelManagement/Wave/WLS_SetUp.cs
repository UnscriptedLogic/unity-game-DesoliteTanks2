using UnityEngine;

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
            string mapToLoad = GameManager.FALLBACK_MAP;
            if (GameManager.hasLevelDetails)
            {
                WLDetailsSO levelDetails = (WLDetailsSO)GameManager.levelDetails;
                wlContext.LevelDetails = levelDetails;
                wlContext.WL_SpawnList = levelDetails.SpawnList;
                mapToLoad = levelDetails.MapName;
            }

            LoadMap(mapToLoad, (scene, loadMode) =>
            {
                GameObject baseGO = GameObject.FindGameObjectWithTag("Base");
                if (baseGO != null)
                {
                    wlContext.BaseLocation = baseGO.transform;
                }

                Debug.Log(wlContext.BaseLocation);
            });
            
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}