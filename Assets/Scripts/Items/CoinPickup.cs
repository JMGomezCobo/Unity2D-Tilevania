using UnityEngine;

namespace Tilevania.Items
{
    public class CoinPickup : MonoBehaviour
    {
        [SerializeField] private AudioClip coinPickupSFX;
        [SerializeField] private int pointsForCoinPickup = 100;

        private bool wasCollected = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player") || wasCollected) return;
            
            wasCollected = true;
                
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
                
            gameObject.SetActive(false);
                
            Destroy(gameObject);
        }
    }
}