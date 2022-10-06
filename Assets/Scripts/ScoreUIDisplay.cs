using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreUIDisplay : MonoBehaviour
    {
        [SerializeField] private string prefix = "Score: ";
        [SerializeField] private string teamID; 
        [SerializeField] private TextMeshProUGUI scoreTMP;
        [SerializeField] private EntityScoreManager scoreManager;

        private void Awake()
        {
            scoreManager.OnScoreModified += UpdateScore;
        }

        private void UpdateScore(Dictionary<string, int> teamScores)
        {
            scoreTMP.text = $"{prefix} {teamScores[teamID]}";
        }
    }
}