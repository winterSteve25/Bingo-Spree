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

        private int _amountLeft;
        private int _amountNeeded;

        public void Init(ItemStack item)
        {
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
        
        public void BingoColor()
        {
            background.color = new Color(242 / 255f, 201 / 255f, 76 / 255f);
        }

        private void UpdateName(int amount)
        {
            text.text = $"{_amountNeeded - amount} / {_amountNeeded}";
        }
    }
}