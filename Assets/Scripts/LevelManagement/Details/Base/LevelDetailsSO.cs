using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    [CreateAssetMenu(fileName = "LevelDetails", menuName = "ScriptableObjects/LevelManagement/New Level Detail")]
    public class LevelDetailsSO : ScriptableObject
    {
        [Header("UI Details")]
        [SerializeField] protected string levelName;
        [SerializeField] protected string levelDifficulty;
        [SerializeField] protected string levelMission;
        [SerializeField] protected string levelStageName;

        [TextArea(5, 10)]
        [SerializeField] protected string levelDesc;

        [SerializeField] protected Sprite levelIcon;
        [SerializeField] protected Sprite levelMap;

        protected int hiscore;

        [Header("Level Loading")]
        [Tooltip("Scene name of gameplay")]
        [SerializeField] protected string gameplayIndex;        
        
        [Tooltip("Scene name of map")]
        [SerializeField] protected string mapIndex;

        public string LevelName => levelName;
        public string Difficulty => levelDifficulty;
        public string Mission => levelMission;
        public string StageName => levelStageName;
        public string Description => levelDesc;
        public Sprite Icon => levelIcon;
        public Sprite MapSprite => levelMap;
        public string ModeName => gameplayIndex;
        public string MapName => mapIndex;
        public int HighScore => hiscore;

        public void SetScore(int score)
        {
            if (score > hiscore)
                hiscore = score;
        }
    }
}