using System;
using UnityEngine;

namespace Items
{
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private bool isHovering;

        public Item Item => item;
        public bool IsHovering
        {
            get => isHovering;
            set
            {
                isHovering = value;
                UpdateBorder();
            }
        }

        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = item.Icon;
        }

        private void UpdateBorder()
        {
            
        }
    }
}