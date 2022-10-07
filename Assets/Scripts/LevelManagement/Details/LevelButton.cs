using LevelManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private LevelDetailsSO levelDetails;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI difficulty;
    [SerializeField] private TextMeshProUGUI hiscore;
    [SerializeField] private Image background;
    [SerializeField] private Image border;
    [SerializeField] private Button button;

    public Button Button => button;

    public void LoadButton(LevelDetailsSO levelDetails)
    {
        icon.sprite = levelDetails.Icon;
        title.text = $"{levelDetails.LevelName} - {levelDetails.MapName}";
        difficulty.text = $"Difficulty: {levelDetails.Difficulty}";
        hiscore.text = $"High Score: {levelDetails.HighScore}";
        this.levelDetails = levelDetails;

        WLDetailsSO wlDetails = (WLDetailsSO)levelDetails;
        background.color = wlDetails.BgColor;
        border.color = wlDetails.BorderColor;
    }
}
