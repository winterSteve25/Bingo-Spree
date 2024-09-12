using DG.Tweening;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private Rigidbody2D rb;

        private Vector2 _moveDirection;

        // Update is called once per frame
        void Update()
        {
            // Processing Inputs
            ProcessInputs();
        }

        void FixedUpdate()
        {
            // Physics calculations
            Move();
        }

        void ProcessInputs()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            _moveDirection = new Vector2(moveX, moveY).normalized;

            if (_moveDirection.sqrMagnitude != 0)
            {
                transform.DORotateQuaternion(
                    Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(_moveDirection.y, _moveDirection.x) - 90), 0.2f);
            }
            else
            {
                transform.DORotate(Vector3.zero, 0.2f);
            }
        }

        void Move()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }
    }
}