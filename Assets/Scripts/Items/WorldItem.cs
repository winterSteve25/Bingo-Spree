using System;
using System.Collections;
using DG.Tweening;
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
                UpdateVisual();
            }
        }

        private SpriteRenderer _sprite;

        private void Start()
        {
            _sprite = GetComponent<SpriteRenderer>();
            if (item != null) Init(item);
        }

        public void Init(Item item)
        {
            _sprite.sprite = item.Icon;
        }

        private void UpdateVisual()
        {
            _sprite.DOColor(isHovering ? Color.cyan : Color.white, 0.1f);
        }
    }
}