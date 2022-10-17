using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    [CreateAssetMenu(fileName = "Item Collection", menuName = "ScriptableObjects/Items/New Item Collection", order = 1)]
    public class ItemCollectionSO : ScriptableObject
    {
        [SerializeField] private List<ItemSO> items;

        public List<ItemSO> Items => items;
    }
}