using Entities;
using Entities.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListensToInventoryUpdate
{
    void OnInventoryUpdated(Entity entity, List<InventorySlot> inventory, InventoryItem recentItem, int recentItemAmount);
}
