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
        }

        void Move()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }
    }
}