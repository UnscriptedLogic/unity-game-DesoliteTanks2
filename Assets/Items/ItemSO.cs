using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item", fileName = "New Item")]
    public class ItemSO : ScriptableObject
    {
        [SerializeField] private int maxStack = 1;
        [SerializeField] private GameObject itemObject;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private Color borderColor;

        [TextArea(5, 10)]
        [SerializeField] private string itemDesc;

        public int MaxStack => maxStack;
        public GameObject ItemObject => itemObject;
        public Sprite Icon => itemIcon;
        public string Description => itemDesc;
        public Color BorderColor => borderColor;
    }
}