using Tilevania.Player;
using UnityEngine;

namespace Tilevania
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 20f;
    
        private Rigidbody2D myRigidbody;
        private PlayerMovement player;
    
        private float xSpeed;

        private void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            player = FindObjectOfType<PlayerMovement>();
            xSpeed = player.transform.localScale.x * bulletSpeed;
        }

        private void Update()
        {
            myRigidbody.velocity = new Vector2 (xSpeed, 0f);
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            if (other.CompareTag("Enemy"))
            {
                Destroy(other.gameObject);
            }
        
            Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other) 
        {
            Destroy(gameObject);    
        }
    }
}