using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("اتصالات UI و آبجکت‌ها")]
    public TextMeshProUGUI scoreText;
    public BallController ball;
    public ShoeController shoe;

    [Header("هدف این مرحله")]
    public int scoreToAchieve;          // امتیازی که بازیکن در این مرحله باید به آن برسد
    public string nextLevelUnlockKey;   // کلید مرحله بعدی که با رسیدن به هدف، باز می‌شود

    private int score = 0;
    private bool gameStarted = false;

    void Start()
    {
        if (scoreText != null) scoreText.text = "0";
    }

    void Update()
    {
        if (!gameStarted && Input.GetMouseButtonDown(0))
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true;
        if (ball != null) ball.StartFalling();
        if (shoe != null) shoe.ActivateMovement();
    }

    public void AddScore()
    {
        score++;
        scoreText.text = score.ToString();

        // اگر بازیکن به امتیاز هدف این مرحله رسید
        if (score >= scoreToAchieve)
        {
            // و اگر کلیدی برای مرحله بعد تعریف شده باشد
            if (!string.IsNullOrEmpty(nextLevelUnlockKey))
            {
                // فقط در صورتی که این کلید قبلاً ثبت نشده، آن را ثبت کن
                if (PlayerPrefs.GetInt(nextLevelUnlockKey, 0) == 0)
                {
                    PlayerPrefs.SetInt(nextLevelUnlockKey, 1);
                    PlayerPrefs.Save();
                    Debug.Log("مرحله " + nextLevelUnlockKey + " باز شد!");
                }
            }
        }
    }
}