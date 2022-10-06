using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Inventory
{
    public class EntityInventory : MonoBehaviour, IInventoryAsLoot
    {
        [Header("Stats")]
        [SerializeField] private int maxSlots = 3;
        [SerializeField] private List<InventorySlot> inventorySlots;

        [Header("Components")]
        [SerializeField] private Entity entityClass;
        [SerializeField] private BaseAttackClass attackClass;
        [SerializeField] private BaseHealthClass healthClass;

        private InventoryItemFactory itemFactory;
        
        public int MaxSlots => maxSlots;
        public List<InventorySlot> InventorySlots => inventorySlots;
        public InventoryItemFactory ItemFactory => itemFactory;
        public BaseAttackClass AttackClass => attackClass;
        public BaseHealthClass HealthClass => healthClass;

        public Action<List<InventorySlot>, Entity> OnInventoryDeath;
        public Action<InventoryItem, int> OnItemAdded;
        public Action<int> OnSlotsModified;

        private void Awake()
        {
            itemFactory = new InventoryItemFactory(this);
            inventorySlots = new List<InventorySlot>();
        }

        private void OnEnable()
        {
            InventoryManager.instance.OnInventoryUpdated?.Invoke(entityClass, inventorySlots, null, 0);
        }

        public void AddItem(InventoryItem item, int amount)
        {
            if (inventorySlots.Count >= maxSlots)
            {
                Debug.Log("Inventory is full");
                return;
            }

            InventorySlot slot = inventorySlots.Find(x => x.item.GetType() == item.GetType());
            if (slot == null)
            {
                slot = new InventorySlot(item);
                inventorySlots.Add(slot);
            }

            if (slot.hasSpace)
            {
                slot.amount += amount;
                slot.item.OnAttached();
            }

            OnItemAdded?.Invoke(item, amount);
            InventoryManager.instance.OnInventoryUpdated?.Invoke(entityClass, inventorySlots, item, amount);
        }

        public void RemoveItem(InventoryItem item, int amount)
        {
            InventorySlot slot = inventorySlots.Find(x => x.item == item);
            if (slot == null)
            {
                Debug.Log("Item not found");
                return;
            }
            
            int amountToDrop = Mathf.Min(slot.amount, amount);
            slot.amount -= amountToDrop;
            for (int i = 0; i < amountToDrop; i++)
            {
                slot.item.OnRemoved();
            }
            
            if (slot.amount <= 0f)
            {
                inventorySlots.Remove(slot);
            }

            InventoryManager.instance.OnInventoryUpdated?.Invoke(entityClass, inventorySlots, item, -amount);
        }

        public void AddSlots(int amount)
        {
            maxSlots += amount;
            OnSlotsModified?.Invoke(maxSlots);
        }

        public void Update()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                inventorySlots[i].item.Update();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                inventorySlots[i].ClearSlot();
            }

            inventorySlots.Clear();
            InventoryManager.instance.OnInventoryUpdated?.Invoke(entityClass, inventorySlots, null, 0);
        }
    }
}