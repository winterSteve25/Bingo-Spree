using System;
using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static PlayerMovement _instance;
        public static PlayerMovement Instance => _instance;
        
        public float moveSpeed;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform playerLoc;

        private Vector2 _moveDirection;
        private bool _side;
        private bool _up;
        private bool _down;
        private static readonly int Side = Animator.StringToHash("Side");
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Up = Animator.StringToHash("Up");

        private void Awake()
        {
            _instance = this;
        }

        void Update()
        {
            ProcessInputs();
            playerLoc.position = transform.position;
        }

        void FixedUpdate()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }

        private void ProcessInputs()
        {
            float moveX = PlayerInput.GetAxisRaw("Horizontal");
            float moveY = PlayerInput.GetAxisRaw("Vertical");

            _moveDirection = new Vector2(moveX, moveY).normalized;

            if (_moveDirection.sqrMagnitude != 0)
            {
                if (Mathf.Abs(_moveDirection.y) > 0 && Mathf.Abs(_moveDirection.x) > 0)
                {
                    transform.DORotateQuaternion(
                        Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(_moveDirection.y), _moveDirection.y < 0 ? -_moveDirection.x : _moveDirection.x) - 90),
                        0.2f);
                }
                else if (_moveDirection.y < 0 && Mathf.Abs(_moveDirection.x) > 0)
                {
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, 90), 0.2f);
                }
                else if (_moveDirection.x < 0 && Mathf.Abs(_moveDirection.y) < 0.0001)
                {
                    transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), 0.2f);
                }
                else
                {
                    transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.2f);
                }
            }
            else
            {
                _side = false;
                _down = false;
                _up = false;
                UpdateVars();
            }

            if (Mathf.Abs(_moveDirection.x) > 0 && !_side)
            {
                _side = true;
                _down = false;
                _up = false;
                UpdateVars();
            }

            if (_moveDirection.y > 0 && !_up)
            {
                _up = true;
                _side = false;
                _down = false;
                UpdateVars();
            }
            
            if (_moveDirection.y < 0 && !_down)
            {
                _up = false;
                _side = false;
                _down = true;
                UpdateVars();
            }

            void UpdateVars()
            {
                animator.SetBool(Side, _side);
                animator.SetBool(Down, _down);
                animator.SetBool(Up, _up);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!other.gameObject.CompareTag("NPC"))
            {
                return;
            }

            PlayerTasks.Instance.CollidedWithNPC = true;
        }
    }
}