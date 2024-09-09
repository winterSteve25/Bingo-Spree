using System;
using System.Collections;
using DG.Tweening;
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
            StartCoroutine(DeleteItem(item));
        }

        private IEnumerator DeleteItem(WorldItem item)
        {
            Transform trans = item.transform;
            float time = 0.2f;
            Destroy(item);

            trans.DOScale(Vector3.zero, time);

            float t = 0;
            Vector3 p0 = trans.position;
            Vector3 p2 = transform.position;
            Vector3 p1 = (p2 - p0) * 0.5f + p0;

            if (Mathf.Abs(p2.x - p0.x) > 0.01)
            {
                p1.y += 1 / (p2.x - p0.x);
            }

            DOTween.To(() => t, x =>
            {
                t = x;
                trans.position = Mathf.Pow(1 - t, 2) * p0 +
                                 2 * (1 - t) * t * p2 +
                                 Mathf.Pow(t, 2) * p1;
            }, 1, time);

            yield return new WaitForSeconds(time);
            Destroy(trans.gameObject);
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