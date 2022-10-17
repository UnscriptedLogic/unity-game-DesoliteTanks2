using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class InventoryItem
    {
        protected EntityInventory inventory;
        protected InventoryItemFactory itemFactory;
        protected ItemSO itemSO;

        public ItemSO ItemScriptable => itemSO;

        public InventoryItem(ItemSO itemSO, EntityInventory inventory, InventoryItemFactory itemFactory)
        {
            this.inventory = inventory;
            this.itemFactory = itemFactory;
            this.itemSO = itemSO;
        }

        public virtual void OnAttached()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void OnRemoved()
        {

        }
    }
}