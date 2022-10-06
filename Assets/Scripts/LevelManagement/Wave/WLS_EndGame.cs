using System;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_EndGame : LevelState
    {
        private bool isGameWon;
        private WaveLevelContext wlContext;

        public WLS_EndGame(LevelStateContext context, LevelStateFactory factory) : base(context, factory)
        {
            wlContext = (WaveLevelContext)context;
        }

        public void Initialize(bool gameWon)
        {
            isGameWon = gameWon;
        }

        public override void EnterState()
        {
            wlContext.EndGameText.text = isGameWon ? "Stage Cleared!" : "Base Destroyed!";
            wlContext.EndGamePage.SetActive(true);
            context.OnEndGameStateChanged?.Invoke(isGameWon);
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            
        }
    }
}
