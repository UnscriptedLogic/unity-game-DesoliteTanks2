using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Inventory
{
    public class InventorySlot
    {
        public InventoryItem item;
        public int amount;
        public bool hasSpace => amount < item.ItemScriptable.MaxStack;

        public InventorySlot(InventoryItem item)
        {
            this.item = item;
            this.amount = 0;
        }

        public void ClearSlot()
        {
            for (int i = 0; i < amount; i++)
            {
                item.OnRemoved();
            }

            amount = 0;
        }
    }
}