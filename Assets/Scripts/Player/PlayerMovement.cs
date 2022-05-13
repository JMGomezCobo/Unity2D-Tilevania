using UnityEngine;
using UnityEngine.InputSystem;

namespace Tilevania.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float _runSpeed = 10f;
        [SerializeField] private float _jumpSpeed = 5f;
        [SerializeField] private float _climbSpeed = 5f;
        
        [SerializeField] private Vector2 _deathKick = new Vector2 (10f, 10f);
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform gun;

        private Vector2 moveInput;
        
        private Animator _animator;
        private BoxCollider2D _boxCollider2D;
        private CapsuleCollider2D _capsuleCollider2D;
        private Rigidbody2D _rigidbody2D;
        
        private float gravityScaleAtStart;

        private bool _isAlive = true;
        
        private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        private static readonly int Dying = Animator.StringToHash("Dying");

        private void Start()
        {
            _animator          = GetComponent<Animator>();
            _boxCollider2D     = GetComponent<BoxCollider2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _rigidbody2D       = GetComponent<Rigidbody2D>();

            gravityScaleAtStart = _rigidbody2D.gravityScale;
        }

        private void Update()
        {
            if (!_isAlive) { return; }
            
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }

        private void OnFire(InputValue value)
        {
            if (!_isAlive) { return; }
            Instantiate(bullet, gun.position, transform.rotation);
        }

        private void OnMove(InputValue value)
        {
            if (!_isAlive) { return; }
            moveInput = value.Get<Vector2>();
        }

        private void OnJump(InputValue value)
        {
            if (!_isAlive) { return; }
            if (!_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}
        
            if(value.isPressed)
            {
                _rigidbody2D.velocity += new Vector2 (0f, _jumpSpeed);
            }
        }

        private void Run()
        {
            var playerVelocity = new Vector2 (moveInput.x * _runSpeed, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = playerVelocity;

            var playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;
            _animator.SetBool(IsRunning, playerHasHorizontalSpeed);

        }

        private void FlipSprite()
        {
            var playerHasHorizontalSpeed = Mathf.Abs(_rigidbody2D.velocity.x) > Mathf.Epsilon;

            if (playerHasHorizontalSpeed)
            {
                transform.localScale = new Vector2 (Mathf.Sign(_rigidbody2D.velocity.x), 1f);
            }
        }

        private void ClimbLadder()
        {
            if (!_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
            { 
                _rigidbody2D.gravityScale = gravityScaleAtStart;
                _animator.SetBool(IsClimbing, false);
                return;
            }
        
            var climbVelocity = new Vector2 (_rigidbody2D.velocity.x, moveInput.y * _climbSpeed);
            _rigidbody2D.velocity = climbVelocity;
            _rigidbody2D.gravityScale = 0f;

            var playerHasVerticalSpeed = Mathf.Abs(_rigidbody2D.velocity.y) > Mathf.Epsilon;
            _animator.SetBool(IsClimbing, playerHasVerticalSpeed);
        }

        private void Die()
        {
            if (!_capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards"))) return;
            
            _isAlive = false;
            _animator.SetTrigger(Dying);
            _rigidbody2D.velocity = _deathKick;
            
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
}