using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public LevelTimer levelTimer;
    public LevelData levelData;
    public Animator playerAnimator;
    public Image fadeImage;
    public Canvas resultCanvas;

    private int currentSmallLevel = 0;
    private bool isFading = false;
    private float fadeDuration = 2f;

    private void Start()
    {
        // 初始化第一个小关卡
        ActivateSmallLevel(0);
    }

    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();
        // 停止人物移动
        // 假设人物移动脚本中有 StopMovement 方法
        playerAnimator.SetTrigger("Clap");

        if (currentSmallLevel < 2)
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else
        {
            levelTimer.StopTimer();
            resultCanvas.gameObject.SetActive(true);
            // 可以在这里添加保存时间等结算逻辑
            float totalTime = levelTimer.GetElapsedTime();
            Debug.Log("大关卡通关时间: " + totalTime + " 秒");
        }
    }

    private System.Collections.IEnumerator FadeAndSwitchLevel()
    {
        isFading = true;
        float elapsedTime = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = Color.white;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;

        // 切换关卡
        DeactivateSmallLevel(currentSmallLevel);
        currentSmallLevel++;
        ActivateSmallLevel(currentSmallLevel);
        // 将人物设置到新关卡的初始位置
        playerAnimator.transform.position = levelData.smallLevelStartPositions[currentSmallLevel];

        elapsedTime = 0f;
        targetColor = Color.clear;

        while (elapsedTime < fadeDuration)
        {
            fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeImage.color = targetColor;
        isFading = false;
    }

    private void ActivateSmallLevel(int levelIndex)
    {
        levelData.smallLevelPlatforms[levelIndex].SetActive(true);
    }

    private void DeactivateSmallLevel(int levelIndex)
    {
        levelData.smallLevelPlatforms[levelIndex].SetActive(false);
    }
}