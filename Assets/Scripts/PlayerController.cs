using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 7f;
    public float flyForce = 2f; // Uçma gücü
    public float maxFlyFuel = 100f; // Maksimum uçma yakıtı
    public float flyFuel = 100f; // Mevcut uçma yakıtı
    public float flyFuelConsumptionRate = 50f; // Yakıt tüketim hızı (saniyede 50)

    public LayerMask groundLayer; // Zemin layer'ı
    public Slider fuelSlider; // Yakıt barı
    public Text coinText; // Coin sayısı gösterimi

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private bool isGrounded;
    private int jumpCount = 1;  // Double jump için izin verilen zıplama sayısı
    private float moveInput;

    private int coin = 0; // Toplanan coin sayısı

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        if (fuelSlider != null)
        {
            fuelSlider.maxValue = maxFlyFuel;
            fuelSlider.value = flyFuel;
        }
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }

    void Update()
    {
        // BoxCollider2D kullanarak zeminle temas kontrolü
        isGrounded = IsGrounded();
        Debug.Log("Is Grounded: " + isGrounded);

        // Zeminle temas ediliyorsa zıplama sayısını sıfırla
        if (isGrounded)
        {
            jumpCount = 1;  // Yere değince zıplama sayısını sıfırla
        }

        moveInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Uçma (F tuşuna basılı tutulduğunda)
        if (Input.GetKey(KeyCode.F) && flyFuel > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flyForce);
            flyFuel -= flyFuelConsumptionRate * Time.deltaTime; // Yakıtı azalt
            isGrounded = false;  // Uçarken yere değmeyebiliriz
        }

        // Yakıtın sıfır olup olmadığını kontrol et
        if (flyFuel <= 0)
        {
            flyFuel = 0; // Yakıtı sıfır yap
        }

        // Slider değerini güncelle
        if (fuelSlider != null)
        {
            fuelSlider.value = flyFuel;
        }

        // Zıplama
        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.F))
        {
            if (isGrounded || jumpCount > 0)  // Zeminle temas varsa veya zıplama hakkı varsa
            {
                Jump();
            }
        }

        // Karakterin yönünü belirleme
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);  // Sağa bak
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Sola bak
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        jumpCount--;
        isGrounded = false;
    }

    bool IsGrounded()
    {
        // Zeminle temas kontrolü
        float colliderHeight = boxCollider.size.y;
        float colliderWidth = boxCollider.size.x;
        Vector2 colliderCenter = boxCollider.bounds.center;

        // Raycast'lerin aşağıya doğru doğru çalıştığından emin olun
        return Physics2D.BoxCast(colliderCenter, new Vector2(colliderWidth, colliderHeight), 0f, Vector2.down, 0.1f, groundLayer);
    }

    public void AddCoins(int amount)
    {
        coin += amount;
        if (coinText != null)
        {
            coinText.text = coin.ToString();
        }
    }

}
