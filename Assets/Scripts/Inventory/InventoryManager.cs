using Entities;
using Entities.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Action<Entity, List<InventorySlot>, InventoryItem, int> OnInventoryUpdated;

    private void Awake()
    {
        instance = this;    
    }
}
