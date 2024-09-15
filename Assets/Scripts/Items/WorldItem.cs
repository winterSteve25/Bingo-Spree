using System;
using System.Collections;
using Audio;
using DG.Tweening;
using UnityEngine;

namespace Items
{
    public class WorldItem : MonoBehaviour
    {
        [SerializeField] private Item item;
        [SerializeField] private bool isHovering;
        
        public bool Pickable => _pickable;
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

        [SerializeField] private SpriteRenderer sprite;
        
        private bool _pickable;
        private Transform _target;

        private void Start()
        {
            _pickable = true;
            if (item != null) Init(item);
        }

        public void Init(Item item)
        {
            sprite.sprite = item.Icon;
            this.item = item;
        }

        public void PickUp(Transform target)
        {
            _pickable = false;
            _target = target;
        }

        private void Update()
        {
            if (_pickable) return;

            var d = (_target.position - transform.position);
            transform.position += d * (Time.deltaTime * (10 / (d.magnitude) + 2));

            if ((transform.position - _target.position).magnitude < 0.5)
            {
                Destroy(gameObject);
            }
        }

        private void UpdateVisual()
        {
            sprite.DOColor(isHovering ? Color.gray : Color.white, 0.1f);
        }
    }
}