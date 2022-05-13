using UnityEngine;

namespace Tilevania.AI
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 1f;
        
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody2D.velocity = new Vector2 (moveSpeed, 0f);
        }

        private void OnTriggerExit2D(Collider2D other) 
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }

        private void FlipEnemyFacing()
        {
            transform.localScale = new Vector2 (-(Mathf.Sign(_rigidbody2D.velocity.x)), 1f);
        }
    }
}