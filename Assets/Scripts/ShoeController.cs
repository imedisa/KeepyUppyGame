using UnityEngine;

public class ShoeController : MonoBehaviour
{
    private Camera mainCamera;
    private bool canMove = false;
    private Rigidbody2D rb;

    private Vector2 lastPosition;
    public Vector2 velocity { get; private set; }

    private float minX, maxX, minY, maxY;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        // محاسبه مرزهای صفحه نمایش
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        minX = -camWidth;
        maxX = camWidth;
        minY = -camHeight;
        
        // ▼▼▼ [این خط تغییر کرده است] ▼▼▼
        maxY = 0; // محدود کردن حرکت عمودی تا وسط صفحه
        // ▲▲▲ [پایان تغییر] ▲▲▲
    }

    void Update()
    {
        if (!canMove) return;

        if (Input.GetMouseButton(0))
        {
            Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // موقعیت کفش را در مرزهای مشخص شده محدود کن
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY); // maxY جدید اینجا اعمال می‌شود

            rb.MovePosition(targetPosition);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            velocity = ((Vector2)transform.position - lastPosition) / Time.fixedDeltaTime;
            lastPosition = transform.position;
        }
    }

    public void ActivateMovement()
    {
        canMove = true;
        lastPosition = transform.position;
    }
}