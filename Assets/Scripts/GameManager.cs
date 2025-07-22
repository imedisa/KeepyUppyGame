using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // --- متغیرهای عمومی (باید در Inspector وصل شوند) ---
    public TextMeshProUGUI scoreText;
    public BallController ball;
    public ShoeController shoe;

    // --- متغیرهای داخلی ---
    private int score = 0;
    private int highScore = 0;
    private bool gameStarted = false;

    void Start()
    {
        // خواندن رکورد از حافظه گوشی
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        
        // تنظیم متن اولیه امتیاز
        if (scoreText != null)
        {
            scoreText.text = "0";
        }
    }

    void Update()
    {
        // منتظر اولین کلیک/لمس برای شروع بازی
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        // این متغیر را true می‌کنیم تا بازی دوباره با کلیک‌های بعدی ری‌استارت نشود
        gameStarted = true;
        
        // به توپ و کفش دستور فعال شدن می‌دهیم
        if (ball != null)
        {
            ball.StartFalling();
        }
        if (shoe != null)
        {
            shoe.ActivateMovement();
        }
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();

        // چک کردن برای ثبت رکورد جدید
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            Debug.Log("رکورد جدید ثبت شد: " + highScore);
        }
    }
}