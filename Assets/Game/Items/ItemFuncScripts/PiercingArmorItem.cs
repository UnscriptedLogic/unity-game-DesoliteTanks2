using System;
using UnityEngine;

namespace Entities.Inventory
{
    public class PiercingArmorItem : InventoryItem
    {
        private int pierce;

        public PiercingArmorItem(ItemSO itemSO, EntityInventory inventory, InventoryItemFactory itemFactory) : base(itemSO, inventory, itemFactory)
        {
        }

        public void Initialize(int pierce)
        {
            this.pierce = pierce;
        }

        public override void OnAttached()
        {
            inventory.AttackClass.Piercing += pierce;
        }

        public override void Update()
        {

        }

        public override void OnRemoved()
        {
            inventory.AttackClass.Piercing -= pierce;
        }
    }
}