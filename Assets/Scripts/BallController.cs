using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    // --- Public Variables ---
    public float hitForceMultiplier = 15f;
    public float velocityImpactMultiplier = 0.5f;
    public float maxHitForce = 25f;
    public GameObject offscreenIndicator;
    public GameManager gameManager;

    // --- Private Variables ---
    private Rigidbody2D rb;
    private Camera mainCamera;
    private float topBound;
    private float screenWidthInWorldUnits;
    private SpriteRenderer indicatorSpriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Calculate screen bounds
        topBound = mainCamera.orthographicSize;
        screenWidthInWorldUnits = topBound * mainCamera.aspect;

        // Hide the indicator and cache its renderer
        if (offscreenIndicator != null)
        {
            offscreenIndicator.SetActive(false);
            indicatorSpriteRenderer = offscreenIndicator.GetComponent<SpriteRenderer>();
        }

        // Set the initial state of the ball
        rb.isKinematic = true;
        rb.simulated = false;
    }

    void Update()
    {
        if (!rb.isKinematic)
        {
            HandleIndicator();
        }
    }

    void HandleIndicator()
    {
        if (offscreenIndicator == null || indicatorSpriteRenderer == null) return;

        if (transform.position.y > topBound)
        {
            if (!offscreenIndicator.activeSelf)
            {
                offscreenIndicator.SetActive(true);
            }

            float indicatorHalfWidth = indicatorSpriteRenderer.bounds.extents.x;
            float indicatorX = Mathf.Clamp(transform.position.x, -screenWidthInWorldUnits + indicatorHalfWidth, screenWidthInWorldUnits - indicatorHalfWidth);
            float indicatorY = topBound - 0.5f;

            offscreenIndicator.transform.position = new Vector2(indicatorX, indicatorY);
        }
        else
        {
            if (offscreenIndicator.activeSelf)
            {
                offscreenIndicator.SetActive(false);
            }
        }
    }

    public void StartFalling()
    {
        rb.isKinematic = false;
        rb.simulated = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("shoe"))
        {
            if (gameManager != null)
            {
                gameManager.AddScore();
            }

            // The rest of the hit logic
            ShoeController shoe = collision.gameObject.GetComponent<ShoeController>();
            if (shoe == null) return;
            rb.velocity = Vector2.zero;
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            if (direction.y < 0.2f)
            {
                direction.y = 0.2f;
            }
            Vector2 shoeVelocity = shoe.velocity;
            float potentialHitForce = hitForceMultiplier + (shoeVelocity.magnitude * velocityImpactMultiplier);
            float finalHitForce = Mathf.Clamp(potentialHitForce, hitForceMultiplier, maxHitForce);
            rb.AddForce(direction.normalized * finalHitForce, ForceMode2D.Impulse);
        }
    }
    
    // Game Over Logic
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            // ▼▼▼ [این خط تغییر کرده است] ▼▼▼
            // به جای شروع مجدد، به منوی اصلی برمی‌گردیم
            SceneManager.LoadScene("MainMenu");
            // ▲▲▲ [پایان تغییر] ▲▲▲
        }
    }
}