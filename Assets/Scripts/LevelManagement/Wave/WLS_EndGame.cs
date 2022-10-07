using System;
using UnityEngine;

namespace LevelManagement
{
    public class WLS_EndGame : LevelState
    {
        private bool isGameWon;
        private WaveLevelContext wlContext;

        private float lerpSpeed;

        private float displayInterval = 1f;
        private float _displayInterval;

        private bool showTimePlayed, showCoinsCollected, showEnemiesKilled, showPlayerDeaths, showFinalScore;

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
            _displayInterval = 1f;

            wlContext.TimePlayedTMP.enabled = false;
            wlContext.CoinsCollectedTMP.enabled = false;
            wlContext.EnemiesKilledTMP.enabled = false;
            wlContext.PlayerDeathTMP.enabled = false;
            wlContext.FinalScoreTMP.enabled = false;
            showTimePlayed = true;

            wlContext.ScoreManager.CalculateScores();

            if (isGameWon)
            {
                wlContext.LevelDetails.SetScore(wlContext.ScoreManager.FinalScore);
            }
        }

        public override void ExitState()
        {

        }

        public override void UpdateState()
        {
            _displayInterval -= Time.deltaTime;
            if (_displayInterval > 0f)
                return;

            if (showTimePlayed)
            {
                wlContext.TimePlayedTMP.text = wlContext.ScoreManager.TimePlayed.ToString("F2");
                wlContext.TimePlayedTMP.enabled = true;
                _displayInterval = displayInterval;
                showTimePlayed = false;
                showCoinsCollected = true;
            }
            else if (showCoinsCollected)
            {
                wlContext.CoinsCollectedTMP.text = $"+10 x {wlContext.ScoreManager.CoinsCollected}";
                wlContext.CoinsCollectedTMP.enabled = true;
                _displayInterval = displayInterval;
                showCoinsCollected = false;
                showEnemiesKilled = true;
            }
            else if (showEnemiesKilled)
            {
                wlContext.EnemiesKilledTMP.text = $"+100 * {wlContext.ScoreManager.EnemiesKilled}";
                wlContext.EnemiesKilledTMP.enabled = true;
                _displayInterval = displayInterval;
                showEnemiesKilled = false;
                showPlayerDeaths = true;
            }
            else if (showPlayerDeaths)
            {
                wlContext.PlayerDeathTMP.text = $"-100 * {wlContext.ScoreManager.PlayerDeaths}";
                wlContext.PlayerDeathTMP.enabled = true;
                _displayInterval = displayInterval;
                showPlayerDeaths = false;
                showFinalScore = true;
            }
            else if (showFinalScore)
            {
                wlContext.FinalScoreTMP.text = wlContext.ScoreManager.FinalScore.ToString();
                wlContext.FinalScoreTMP.enabled = true;
                _displayInterval = displayInterval;
                showFinalScore = false;
            }
        }
    }
}
