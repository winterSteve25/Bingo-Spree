using System;
using Items;
using UnityEngine;

namespace Player
{
    public class PlayerPickup : MonoBehaviour
    {
        private static PlayerPickup _instance;
        public static PlayerPickup Instance => _instance;
        
        [SerializeField] private Camera mainCam;
        public float pickupRange;

        public event Action<Item> OnItemPickedUp;

        private WorldItem _hoveringItem;

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            TryPickupItem();
        }

        private void TryPickupItem()
        {
            Vector2 mp = Input.mousePosition;
            Vector2 pos = mainCam.ScreenToWorldPoint(mp);
            Collider2D col = Physics2D.OverlapPoint(pos, LayerMask.GetMask("Items"));

            #region NotHovering

            if (col == null)
            {
                ResetHovering();
                return;
            }

            if (!col.gameObject.TryGetComponent(out WorldItem item))
            {
                ResetHovering();
                return;
            }

            if (!item.Pickable)
            {
                ResetHovering();
                return;
            }

            #endregion

            if (_hoveringItem != item)
            {
                if (_hoveringItem != null)
                {
                    _hoveringItem.IsHovering = false;
                }

                _hoveringItem = item;
                _hoveringItem.IsHovering = true;
            }

            if (!Input.GetMouseButton(0)) return;
            if ((_hoveringItem.transform.position - transform.position).magnitude > pickupRange) return;

            OnItemPickedUp?.Invoke(item.Item);
            item.PickUp(transform);
        }

        private void ResetHovering()
        {
            if (_hoveringItem != null)
            {
                _hoveringItem.IsHovering = false;
            }

            _hoveringItem = null;
        }
    }
}