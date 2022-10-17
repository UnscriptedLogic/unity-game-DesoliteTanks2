using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class DamageItem : InventoryItem
    {
        private float damage;

        public DamageItem(ItemSO itemSO, EntityInventory inventory, InventoryItemFactory itemFactory) : base(itemSO, inventory, itemFactory)
        {
        }

        public void Initialize(float damage)
        {
            this.damage = damage;
        }

        public override void OnAttached()
        {
            inventory.AttackClass.Damage += damage;
        }

        public override void Update()
        {

        }

        public override void OnRemoved()
        {
            inventory.AttackClass.Damage -= damage;
        }
    }
}