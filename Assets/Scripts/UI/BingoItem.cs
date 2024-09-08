using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tasks
{
    public class BingoItem : MonoBehaviour
    {
        private ItemStack _item;
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text;

        public void Init(ItemStack item)
        {
            _item = item;
            image.sprite = item.Item.Icon;
            text.text = $"{item.Item.Name} x{item.Amount}";
        }
    }
}