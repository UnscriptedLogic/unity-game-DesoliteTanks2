using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Inventory
{
    public class InventoryItemUI : MonoBehaviour
    {
        private InventoryItem item;
        [SerializeField] private Image border;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amountText;

        public InventoryItem InventoryItem => item;

        public void SetItem(InventoryItem item, int amount, Color color)
        {
            this.item = item;
            icon.sprite = item.ItemScriptable.Icon;
            amountText.text = amount.ToString();

            amountText.enabled = amount > 1;
            border.color = color;
        }
    }
}