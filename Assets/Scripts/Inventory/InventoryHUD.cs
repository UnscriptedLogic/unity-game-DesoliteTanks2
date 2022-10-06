using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class InventoryHUD : MonoBehaviour, IListensToInventoryUpdate
    {
        [SerializeField] private string entityID = "player";
        [SerializeField] private GameObject itemprefab;
        [SerializeField] private Transform contentParent;

        private void Start()
        {
            InventoryManager.instance.OnInventoryUpdated += OnInventoryUpdated;
        }

        public void OnInventoryUpdated(Entity entity, List<InventorySlot> inventory, InventoryItem recentItem, int recentItemAmount)
        {
            if (!entity.EntityID.Contains(entityID)) return;
            
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }

            foreach (InventorySlot slot in inventory)
            {
                GameObject item = Instantiate(itemprefab, contentParent);
                item.GetComponent<InventoryItemUI>().SetItem(slot.item, slot.amount, slot.item.ItemScriptable.BorderColor);
            }
        }
    }
}