using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class LevelData
{
    public Sprite displayImage;
    public int scoreToUnlock;        // این عدد فقط برای نمایش به بازیکن است
    public string unlockKey;         // کد با این کلید، باز بودن مرحله را چک می‌کند
    public string sceneNameToLoad;
}

public class MenuManager : MonoBehaviour
{
    public List<LevelData> allLevels;

    [Header("UI Placeholders")]
    public Image prevBallImage;
    public Image currentBallImage;
    public Image nextBallImage;
    public TextMeshProUGUI unlockScoreText;
    public Button startButton;
    public Button nextButton;
    public Button prevButton;

    private int currentLevelIndex = 0;

    void Start()
    {
        UpdateUI();
        // PlayerPrefs.DeleteAll();
    }

    public void NextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= allLevels.Count) currentLevelIndex = 0;
        UpdateUI();
    }

    public void PreviousLevel()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0) currentLevelIndex = allLevels.Count - 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (allLevels == null || allLevels.Count == 0) return;
        UpdateBallDisplay(currentBallImage, unlockScoreText, startButton, currentLevelIndex);
        UpdateSideBallDisplay(prevBallImage, currentLevelIndex - 1);
        UpdateSideBallDisplay(nextBallImage, currentLevelIndex + 1);
    }

    void UpdateBallDisplay(Image display, TextMeshProUGUI requirementText, Button button, int index)
    {
        LevelData theme = allLevels[index];
        display.sprite = theme.displayImage;

        // مرحله اول همیشه باز است، برای بقیه مراحل کلیدشان را چک می‌کنیم
        bool isUnlocked = (index == 0) || (PlayerPrefs.GetInt(theme.unlockKey, 0) == 1);

        if (isUnlocked)
        {
            button.interactable = true;
            display.color = Color.white;
            requirementText.gameObject.SetActive(false);
        }
        else
        {
            button.interactable = false;
            display.color = Color.grey;
            requirementText.gameObject.SetActive(true);
            requirementText.text = theme.scoreToUnlock.ToString();
        }
    }

    void UpdateSideBallDisplay(Image sideImage, int index)
    {
        if (index >= 0 && index < allLevels.Count)
        {
            sideImage.gameObject.SetActive(true);
            LevelData theme = allLevels[index];
            sideImage.sprite = theme.displayImage;
            bool isUnlocked = (index == 0) || (PlayerPrefs.GetInt(theme.unlockKey, 0) == 1);
            sideImage.color = isUnlocked ? Color.white : Color.grey;
        }
        else
        {
            sideImage.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        string sceneToLoad = allLevels[currentLevelIndex].sceneNameToLoad;
        SceneManager.LoadScene(sceneToLoad);
    }
}