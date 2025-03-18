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
        // ��ʼ����һ��С�ؿ�
        ActivateSmallLevel(0);
    }

    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();
        // ֹͣ�����ƶ�
        // ���������ƶ��ű����� StopMovement ����
        playerAnimator.SetTrigger("Clap");

        if (currentSmallLevel < 2)
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else
        {
            levelTimer.StopTimer();
            resultCanvas.gameObject.SetActive(true);
            // ������������ӱ���ʱ��Ƚ����߼�
            float totalTime = levelTimer.GetElapsedTime();
            Debug.Log("��ؿ�ͨ��ʱ��: " + totalTime + " ��");
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

        // �л��ؿ�
        DeactivateSmallLevel(currentSmallLevel);
        currentSmallLevel++;
        ActivateSmallLevel(currentSmallLevel);
        // ���������õ��¹ؿ��ĳ�ʼλ��
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