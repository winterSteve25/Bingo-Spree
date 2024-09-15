using Player;
using Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ComboTracker : MonoBehaviour
    {
        private float _combo;
        private Tier _tier;

        private float _baseMovementSpeed;
        private float _basePickupRange;

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

        [SerializeField] private TMP_Text bonusPrefab;

        private void Start()
        {
            _combo = 0;
            _tier = Tier.B;

            _baseMovementSpeed = PlayerMovement.Instance.moveSpeed;
            _basePickupRange = PlayerPickup.Instance.pickupRange;
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
            
            switch (_combo)
            {
                case <= 5 when _tier != Tier.B:
                    PlayerMovement.Instance.moveSpeed = 1.1f * _baseMovementSpeed;
                    PlayerPickup.Instance.pickupRange = 1.1f * _basePickupRange;
                    _tier = Tier.B;
                    effectLeft.minValue = 5;
                    effectLeft.maxValue = 10;
                    break;
                case <= 10 when _tier != Tier.A:
                    PlayerMovement.Instance.moveSpeed = 1.2f * _baseMovementSpeed;
                    PlayerPickup.Instance.pickupRange = 1.2f * _basePickupRange;
                    _tier = Tier.A;
                    effectLeft.minValue = 10;
                    effectLeft.maxValue = 15;
                    break;
                case <= 15 when _tier != Tier.S:
                    PlayerMovement.Instance.moveSpeed = 1.4f * _baseMovementSpeed;
                    PlayerPickup.Instance.pickupRange = 1.4f * _basePickupRange;
                    _tier = Tier.S;
                    effectLeft.minValue = 15;
                    effectLeft.maxValue = 25;
                    break;
                case <= 25 when _tier != Tier.SS:
                    PlayerMovement.Instance.moveSpeed = 1.6f * _baseMovementSpeed;
                    PlayerPickup.Instance.pickupRange = 1.6f * _basePickupRange;
                    _tier = Tier.SS;
                    effectLeft.minValue = 25;
                    effectLeft.maxValue = 25;
                    break;
            }

            effectLeft.value = _combo;
        }

        private enum Tier
        {
            B,
            A,
            S,
            SS,
        }
    }
}