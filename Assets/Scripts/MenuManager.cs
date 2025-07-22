using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro; // این خط باید حتما وجود داشته باشد

[System.Serializable]
public class LevelData
{
    public Sprite displayImage;
    public int scoreToUnlock;
    public string sceneNameToLoad;
}

public class MenuManager : MonoBehaviour
{
    public List<LevelData> allLevels;

    [Header("UI Placeholders")]
    public Image prevBallImage;
    public Image currentBallImage;
    public Image nextBallImage;
    public Button startButton;
    public TextMeshProUGUI unlockScoreText; // <<-- [خط جدید ۱]: فیلد برای متن امتیاز لازم

    private int currentLevelIndex = 0;
    private int highScore = 0;

    void Start()
    {
        // PlayerPrefs.DeleteKey("HighScore"); // این خط باید کامنت باشد مگر برای تست
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= allLevels.Count)
        {
            currentLevelIndex = 0;
        }
        UpdateUI();
    }

    public void PreviousLevel()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0)
        {
            currentLevelIndex = allLevels.Count - 1;
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        if (allLevels == null || allLevels.Count == 0) return;

        LevelData currentTheme = allLevels[currentLevelIndex];
        currentBallImage.sprite = currentTheme.displayImage;

        if (highScore >= currentTheme.scoreToUnlock)
        {
            // مرحله باز است
            startButton.interactable = true;
            currentBallImage.color = Color.white;
            unlockScoreText.gameObject.SetActive(false); // <<-- [خط جدید ۲]: متن امتیاز را مخفی کن
        }
        else
        {
            // مرحله قفل است
            startButton.interactable = false;
            currentBallImage.color = Color.grey;
            unlockScoreText.gameObject.SetActive(true); // <<-- [خط جدید ۳]: متن امتیاز را نمایش بده
            unlockScoreText.text = currentTheme.scoreToUnlock.ToString(); // <<-- [خط جدید ۴]: مقدار امتیاز لازم را در متن بنویس
        }

        // ... بقیه کد برای توپ‌های کناری بدون تغییر باقی می‌ماند ...
        UpdateSideBall(prevBallImage, currentLevelIndex - 1);
        UpdateSideBall(nextBallImage, currentLevelIndex + 1);
    }

    void UpdateSideBall(Image sideImage, int index)
    {
        if (index >= 0 && index < allLevels.Count)
        {
            sideImage.gameObject.SetActive(true);
            sideImage.sprite = allLevels[index].displayImage;
            sideImage.color = (highScore >= allLevels[index].scoreToUnlock) ? Color.white : Color.grey;
        }
        else
        {
            sideImage.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        if (allLevels.Count > 0)
        {
            string sceneToLoad = allLevels[currentLevelIndex].sceneNameToLoad;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}