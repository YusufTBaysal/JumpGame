using UnityEngine;

public class CoinController : MonoBehaviour
{
    public int coinValue = 1; // Coin'in deðerini belirler

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Eðer çarpan nesne "Player" ise
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddCoins(coinValue); // Oyuncuya coin ekle
                Destroy(); // Coin'i yok et
            }
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
