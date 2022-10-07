using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevelManagement
{
    public class MMS_SetUp : LS_LevelSetUp
    {
        private MainMenuContext mmContext;
        private MainMenuFactory mmFactory;

        public MMS_SetUp(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            mmContext = (MainMenuContext)context;
            mmFactory = (MainMenuFactory)factory;
        }

        public override void EnterState()
        {
            for (int i = 0; i < mmContext.WaveLevelParent.childCount; i++)
            {
                UnityEngine.Object.Destroy(mmContext.WaveLevelParent.GetChild(0).gameObject);
            }

            WLDetailsSO wlDetails;
            for (int i = 0; i < mmContext.WLDetails.Count; i++)
            {
                Transform newbutton = UnityEngine.Object.Instantiate(mmContext.LevelButtonPrefab, mmContext.WaveLevelParent).transform;
                wlDetails = mmContext.WLDetails[i];

                LevelButton levelButton = newbutton.GetComponent<LevelButton>();
                levelButton.LoadButton(wlDetails);
                
                levelButton.Button.onClick.AddListener(() =>
                {
                    mmContext.LevelViewPage.SetActive(true);
                    mmContext.LevelSelectPage.SetActive(false);
                    mmContext.WaveLevelView.Display(wlDetails);
                    
                    mmContext.WaveLevelView.PlayButton.onClick.AddListener(() =>
                    {
                        GameManager.levelDetails = wlDetails;
                        mmContext.SceneChanger.MoveToScene(wlDetails);
                    });
                });
            }

            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }
    }
}
