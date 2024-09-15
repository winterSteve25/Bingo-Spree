using System;
using DG.Tweening;
using Player;
using Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComboTracker : MonoBehaviour
    {
        [SerializeField] private float _combo;
        private Tier _tier;

        private float _baseMovementSpeed;
        private float _basePickupRange;

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform list;
        
        [SerializeField] private RectTransform bTitle;
        [SerializeField] private Color bColor;
        [SerializeField] private RectTransform aTitle;
        [SerializeField] private Color aColor;
        [SerializeField] private RectTransform sTitle;
        [SerializeField] private Color sColor;
        [SerializeField] private RectTransform ssTitle;
        [SerializeField] private Color ssColor;
        [SerializeField] private Slider effectLeft;
        [SerializeField] private Image effectLeftSliderColor;
        
        [SerializeField] private TMP_Text effect1Txt;
        [SerializeField] private TMP_Text effect2Txt;

        private void Start()
        {
            _combo = 0;
            _tier = Tier.C;

            _baseMovementSpeed = PlayerMovement.Instance.moveSpeed;
            _basePickupRange = PlayerPickup.Instance.pickupRange;
            canvasGroup.alpha = 0;
            
            UpdateTier();
        }

        private void OnEnable()
        {
            BingoTask.OnBingo += OnBingo;
            BingoTask.OnSlot += OnBingoSlot;
        }

        private void OnDisable()
        {
            BingoTask.OnBingo -= OnBingo;
            BingoTask.OnSlot -= OnBingoSlot;
        }

        private void OnBingo()
        {
            _combo += 10;
            UpdateTier();
        }

        private void OnBingoSlot()
        {
            _combo += 2;
            UpdateTier();
        }

        private void Update()
        {
            if (_combo <= 0)
            {
                _combo = 0;
                return;
            }
            
            _combo -= Time.deltaTime / 3f;
            UpdateTier();
        }

        private void UpdateTier()
        {
            if (_combo > 25)
            {
                _combo = 25;
            }

            float effectPerct = 1;
            
            switch (_combo)
            {
                case <= 0:
                    canvasGroup.DOFade(0, 0.2f);
                    _tier = Tier.C;
                    break;
                case > 0 and <= 1 when _tier != Tier.B:
                    canvasGroup.DOFade(1, 0.2f);
                    _tier = Tier.B;
                    effectLeft.minValue = 0;
                    effectLeft.maxValue = 1;
                    effectLeftSliderColor.color = bColor;
                    aTitle.gameObject.SetActive(false);
                    sTitle.gameObject.SetActive(false);
                    ssTitle.gameObject.SetActive(false);
                    bTitle.gameObject.SetActive(true);
                    break;
                case > 1 and <= 10 when _tier != Tier.A:
                    canvasGroup.DOFade(1, 0.2f);
                    _tier = Tier.A;
                    effectLeft.minValue = 1;
                    effectLeft.maxValue = 10;
                    effectLeftSliderColor.color = aColor;
                    aTitle.gameObject.SetActive(true);
                    sTitle.gameObject.SetActive(false);
                    ssTitle.gameObject.SetActive(false);
                    bTitle.gameObject.SetActive(false);
                    break;
                case > 10 and <= 15 when _tier != Tier.S:
                    canvasGroup.DOFade(1, 0.2f);
                    _tier = Tier.S;
                    effectLeft.minValue = 10;
                    effectLeft.maxValue = 15;
                    effectLeftSliderColor.color = sColor;
                    aTitle.gameObject.SetActive(false);
                    sTitle.gameObject.SetActive(true);
                    ssTitle.gameObject.SetActive(false);
                    bTitle.gameObject.SetActive(false);
                    break;
                case > 15 when _tier != Tier.SS:
                    canvasGroup.DOFade(1, 0.2f);
                    _tier = Tier.SS;
                    effectLeft.minValue = 15;
                    effectLeft.maxValue = _combo * 2;
                    effectLeftSliderColor.color = ssColor;
                    aTitle.gameObject.SetActive(false);
                    sTitle.gameObject.SetActive(false);
                    ssTitle.gameObject.SetActive(true);
                    bTitle.gameObject.SetActive(false);
                    break;
            }

            effectLeft.value = _combo;
            
            effectPerct = _tier switch
            {
                Tier.C => 1.0f,
                Tier.B => 1.1f,
                Tier.A => 1.3f,
                Tier.S => 1.5f,
                Tier.SS => 1.8f,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            PlayerMovement.Instance.moveSpeed = _baseMovementSpeed * effectPerct;
            PlayerPickup.Instance.pickupRange = _basePickupRange * effectPerct;
            
            effect1Txt.text = $"+{(effectPerct - 1) * 100:N0}% Speed";
            effect2Txt.text = $"+{(effectPerct - 1) * 100:N0}% Range";

            LayoutRebuilder.ForceRebuildLayoutImmediate(list);
        }

        private enum Tier
        {
            C,
            B,
            A,
            S,
            SS,
        }
    }
}