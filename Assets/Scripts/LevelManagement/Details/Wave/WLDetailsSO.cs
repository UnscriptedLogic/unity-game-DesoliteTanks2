using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelManagement
{
    [CreateAssetMenu(fileName = "New Wave Level Detail", menuName = "ScriptableObjects/LevelManagement/New Wave Level Detail")]
    public class WLDetailsSO : LevelDetailsSO
    {
        [Header("Wave Level Extension")]
        [SerializeField] private Color bgColor;
        [SerializeField] private Color borderColor;
        [SerializeField] private List<WL_SpawnList> wl_SpawnLists = new List<WL_SpawnList>();

        public Color BgColor => bgColor;
        public Color BorderColor => borderColor; 
        public List<WL_SpawnList> SpawnList => wl_SpawnLists;
    }
}