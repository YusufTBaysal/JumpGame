using UnityEngine;

public class FuelController : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        // PlayerController'� bul
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController component not found on the Player object.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && playerController != null)
        {
            playerController.flyFuel += 10;
            if (playerController.flyFuel > playerController.maxFlyFuel)
            {
                playerController.flyFuel = playerController.maxFlyFuel; // Yak�t�n maksimum de�erini ge�mesini engelle
            }
            Destroy(gameObject);
        }
    }
}
