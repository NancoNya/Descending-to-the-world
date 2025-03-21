using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class LevelManager : Singleton<LevelManager>
{
    [Header("��ʱ��")]
    public LevelTimer levelTimer;

    [Header("���ﶯ��")]
    private Animator playerAnimator;

    [Header("С�ؿ��л��Ľ�������")]
    public CanvasGroup fadeCanvasGroup;  // ����alphaֵʵ���ڵ�
    public float fadeDuration;  // ���������ĳ���ʱ��
    [SerializeField]private bool isFading;

    [Header("��С�ؿ��л�")]
    [SerializeField] private int currentBigLevel = 1;   // ������ؿ�
    [SerializeField] private int currentSmallLevel = 1;   // ����С�ؿ�
    public Button nextLevelButton;   // ͨ�ش�ؿ������ǰ����һ����ؿ�
    public TextMeshProUGUI resultTimeText;   // ��ʾ��ؿ�ͨ��ʱ��
    public Canvas resultCanvas;   // ������棨ͨ�ش�ؿ�ʱ���֣�


    private void Start()
    {
        resultCanvas.gameObject.SetActive(false);
        nextLevelButton.onClick.AddListener(LoadNextBigLevel);

        // ��ȡ���ﶯ��
        if (GameObject.FindWithTag("Player"))
            playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        else
            Debug.Log("didn't find PlayerAnimator");
    }

    /// <summary>
    /// ����С�ؿ��յ�
    /// </summary>
    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)   // ֹͣ�����ƶ������Ź��ƶ���
        {
            player.SetHorizontalVelocityZero();
            player.ClapAnim();
        }
        else
            Debug.Log("did't find PlayerController");

        if (currentSmallLevel < 3 && !isFading)   // �����һ��ڶ���С�ؿ����յ㣬�Ҳ�����fade״̬��ֱ�Ӵ�������һ��С�ؿ���
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else   // ����������յ㣬��ʾ�������
        {
            Debug.Log("1");
            levelTimer.StopTimer();
            Debug.Log("2");
            resultCanvas.gameObject.SetActive(true);
            Debug.Log("3");
            ShowResultCanvas();
            Debug.Log("4");
            currentBigLevel++;
            Debug.Log("5");
            currentSmallLevel = 1;
            Debug.Log("6");
        }
    }

    /// <summary>
    /// С�ؿ��л�
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator FadeAndSwitchLevel()
    {
        yield return Fade(1);
        //isFading = true;
        //float elapsedTime = 0f;  // fade����ʱ��
        //Color startColor = fadeImage.color;
        //Color targetColor = Color.white;

        //while (elapsedTime < fadeDuration)  // ����
        //{
        //    fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        //fadeImage.color = targetColor;

        // �л��ؿ�
        string nextSceneName = $"GameScene{currentBigLevel}.{currentSmallLevel}";   // GameSceneX.X ָ�ؿ��������֣��ɸ��ݺ��������������
        currentSmallLevel++;
        SceneManager.LoadScene(nextSceneName);
        // ���������õ��¹ؿ��ĳ�ʼλ��
        //playerAnimator.transform.position = levelData.smallLevelStartPositions[currentSmallLevel];

        //elapsedTime = 0f;
        //targetColor = Color.clear;

        //while (elapsedTime < fadeDuration)   // ����
        //{
        //    fadeImage.color = Color.Lerp(startColor, targetColor, elapsedTime / fadeDuration);
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}
        //fadeImage.color = targetColor;
        //isFading = false;
        yield return Fade(0);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFading = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        //��������͸���ȣ����ϵ���Mathf.MoveTowards����
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        isFading = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ͨ�ش�ؿ�����ʾ�������
    /// </summary>
    private void ShowResultCanvas()
    {
        resultCanvas.gameObject.SetActive(true);
        float totalTime = levelTimer.GetElapsedTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);
        resultTimeText.text = string.Format("ͨ��ʱ��: {0:00}:{1:00}", minutes, seconds);
        // ������������ӱ���ʱ��Ƚ����߼�
    }

    private void LoadNextBigLevel()
    {
        resultCanvas.gameObject.SetActive(false);
        levelTimer.ResetTimer();
        currentSmallLevel = 1;
        string nextSceneName = $"GameScene{currentBigLevel}.{currentSmallLevel}";
        SceneManager.LoadScene(nextSceneName);
    }
}