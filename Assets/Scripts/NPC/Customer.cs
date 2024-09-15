using System;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC
{
    public class Customer : MonoBehaviour
    {
        private static readonly int Side = Animator.StringToHash("Side");
        private static readonly int Down = Animator.StringToHash("Down");
        private static readonly int Up = Animator.StringToHash("Up");

        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed = 5;
        [SerializeField] private Transform front;

        private Vector2 _moveDirection;
        private bool _side;
        private bool _up;
        private bool _down;
        private Path _pathToFollow;
        
        private float _waitTime;
        private float _waitCooldown;

        private int _nextVertexIndex;

        private void Start()
        {
            _pathToFollow = LevelData.Instance.npcPaths[Random.Range(0, LevelData.Instance.npcPaths.Length)];
            _nextVertexIndex = 0;
        }

        private void Update()
        {
            UpdateVisual();
            
            if (_waitTime > 0)
            {
                _waitTime -= Time.deltaTime;
                _moveDirection = Vector2.zero;
                return;
            }

            _waitTime = 0;
            _waitCooldown -= Time.deltaTime;

            if (_waitCooldown <= 0)
            {
                _waitTime = Random.Range(0.5f, 2f);
                _waitCooldown = Random.Range(2f, 8f);
            }
            
            #region MoveOnPath

            if (_nextVertexIndex >= _pathToFollow.vertices.Length)
            {
                Despawn();
                return;
            }

            if ((_pathToFollow.vertices[_nextVertexIndex] - (Vector2)transform.position).magnitude < 0.5f)
            {
                _nextVertexIndex++;
                if (_nextVertexIndex >= _pathToFollow.vertices.Length)
                {
                    Despawn();
                    return;
                }
            }

            Vector2 pos = _pathToFollow.vertices[_nextVertexIndex];
            _moveDirection = (pos - (Vector2)transform.position).normalized;

            // Collider2D hit = Physics2D.OverlapPoint(front.position);
            // if (hit != null && hit.gameObject != null)
            // {
            //     _moveDirection = Vector2.zero;
            // }
            
            #endregion
        }

        private void Despawn()
        {
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        }

        private void UpdateVisual()
        {
            if (_moveDirection.sqrMagnitude != 0)
            {
                if (Mathf.Abs(_moveDirection.y) > 0 && Mathf.Abs(_moveDirection.x) > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0,
                        Mathf.Rad2Deg * Mathf.Atan2(Mathf.Abs(_moveDirection.y),
                            _moveDirection.y < 0 ? -_moveDirection.x : _moveDirection.x) - 90);
                }
                else if (_moveDirection.y < 0 && Mathf.Abs(_moveDirection.x) > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                else if (_moveDirection.x < 0 && Mathf.Abs(_moveDirection.y) < 0.0001)
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
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
    }
}