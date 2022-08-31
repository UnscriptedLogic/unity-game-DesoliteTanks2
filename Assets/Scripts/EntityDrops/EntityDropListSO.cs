using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/New Entity Drop List", fileName = "New Entity Drop List")]
    public class EntityDropListSO : ScriptableObject
    {
        [Serializable]
        public class DropList
        {
            public string tankID;
            public List<GameObject> entityDrops;
        }

        [SerializeField] private List<DropList> dropList = new List<DropList>();

        public List<DropList> EntityDropList => dropList;
    }
}