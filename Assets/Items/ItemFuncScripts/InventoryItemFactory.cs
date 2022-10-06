using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class InventoryItemFactory
    {
        private EntityInventory context;

        public InventoryItemFactory(EntityInventory context)
        {
            this.context = context;
        }

        public PiercingArmorItem ArmorPiercingAmmo(ItemSO itemSO, int amount)
        {
            PiercingArmorItem piercingArmorItem = new PiercingArmorItem(itemSO, context, this);
            piercingArmorItem.Initialize(amount);
            return piercingArmorItem;
        }

        public DamageItem DamageUpgrade(ItemSO itemSO, float damage)
        {
            DamageItem damageItem = new DamageItem(itemSO, context, this);
            damageItem.Initialize(damage);
            return damageItem;
        }
    }
}