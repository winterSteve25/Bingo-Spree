using System;
using Items;
using UnityEngine;

namespace Player
{
    public class PlayerPickup : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;

        public event Action<Item> OnItemPickedUp;

        private WorldItem _hoveringItem;

        private void Update()
        {
            TryPickupItem();
        }

        private void TryPickupItem()
        {
            Vector2 mp = Input.mousePosition;
            Vector2 pos = mainCam.ScreenToWorldPoint(mp);
            Collider2D col = Physics2D.OverlapPoint(pos);

            #region NotHovering

            if (col == null)
            {
                ResetHovering();
                return;
            }

            if (col.gameObject == null)
            {
                ResetHovering();
                return;
            }

            if (!col.gameObject.TryGetComponent(out WorldItem item))
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

            if (!Input.GetMouseButtonDown(0)) return;
            OnItemPickedUp?.Invoke(item.Item);
            Destroy(item.gameObject);
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