using Audio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tasks
{
    public class GameButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Image underline;
        [SerializeField] private UnityEvent onClick;

        private bool _hovering;

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hovering = true;
            eventData.selectedObject = gameObject;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hovering = false;
            eventData.selectedObject = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.1f)
                .SetEase(Ease.OutCubic);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_hovering)
            {
                transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.1f)
                    .SetEase(Ease.OutCubic);
            }
            else
            {
                transform.DOScale(new Vector3(1f, 1f, 1), 0.1f)
                    .SetEase(Ease.OutCubic);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            AudioManager.Play(6, SourceType.UI);
            
            if (underline != null)
            {
                DOTween.To(
                    () => underline.rectTransform.offsetMin,
                    x => underline.rectTransform.offsetMin = x,
                    new Vector2(0, underline.rectTransform.offsetMin.y),
                    0.2f
                ).SetEase(Ease.OutCubic);

                DOTween.To(
                    () => underline.rectTransform.offsetMax,
                    x => underline.rectTransform.offsetMax = x,
                    new Vector2(0, underline.rectTransform.offsetMax.y),
                    0.2f
                ).SetEase(Ease.OutCubic);
            }

            transform.DOScale(new Vector3(1.3f, 1.3f, 1), 0.1f)
                .SetEase(Ease.OutCubic);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (underline != null)
            {
                float halfWidth = ((RectTransform)transform).rect.width / 2f;

                DOTween.To(
                    () => underline.rectTransform.offsetMin,
                    x => underline.rectTransform.offsetMin = x,
                    new Vector2(halfWidth, underline.rectTransform.offsetMin.y),
                    0.2f
                ).SetEase(Ease.InCubic);

                DOTween.To(
                    () => underline.rectTransform.offsetMax,
                    x => underline.rectTransform.offsetMax = x,
                    new Vector2(-halfWidth, underline.rectTransform.offsetMax.y),
                    0.2f
                ).SetEase(Ease.InCubic);
            }

            transform.DOScale(new Vector3(1f, 1f, 1), 0.2f)
                .SetEase(Ease.InCubic);
        }
    }
}