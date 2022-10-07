using LevelManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveLevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionTMP;
    [SerializeField] private TextMeshProUGUI stageTMP;
    [SerializeField] private TextMeshProUGUI mapNameTMP;
    [SerializeField] private TextMeshProUGUI difficultyTMP;
    [SerializeField] private TextMeshProUGUI descriptionTMP;
    [SerializeField] private TextMeshProUGUI hiscoreTMP;
    [SerializeField] private Image mapView;
    [SerializeField] private Button playButton;

    public Button PlayButton => playButton;

    public void Display(LevelDetailsSO levelDetails)
    {
        missionTMP.text = levelDetails.Mission;
        stageTMP.text = levelDetails.StageName;
        mapNameTMP.text = $"Map: {levelDetails.MapName}";
        difficultyTMP.text = $"Difficulty: {levelDetails.Difficulty}";
        descriptionTMP.text = levelDetails.Description;
        hiscoreTMP.text = levelDetails.HighScore.ToString();
    }
}
