using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class BingoItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;

        private Item _item;
        private int _amountLeft;
        private int _amountNeeded;

        public void Init(ItemStack item)
        {
            _item = item.Item;
            _amountNeeded = item.amount;
            image.sprite = item.Item.Icon;
            UpdateName(item.amount);
        }

        public void UpdateAmount(int amount)
        {
            _amountLeft = amount;
            Color c = Color.green * 0.65f * ((_amountNeeded - _amountLeft) / (float)_amountNeeded);
            background.color = c;
            UpdateName(amount);
        }

        private void UpdateName(int amount)
        {
            text.text = amount > 0 ? $"{_item.Name} x{amount}" : "Completed";
        }
    }
}