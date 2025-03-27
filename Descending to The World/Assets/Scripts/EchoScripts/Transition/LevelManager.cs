using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
using UnityEditor.SearchService;
using Scene = UnityEngine.SceneManagement.Scene;

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
    public LevelInitialSO levelInitialData;  // �������С�����ĳ�ʼ���ݣ�λ�ã�


    private void Start()
    {
        resultCanvas.gameObject.SetActive(false);
        nextLevelButton.onClick.AddListener(StartLoadNextBigLevel);

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
            //player.ClapAnim();
        }
        else
            Debug.Log("did't find PlayerController");

        if (currentSmallLevel < 2 && !isFading)   // �����һ��ڶ���С�ؿ����յ㣬�Ҳ�����fade״̬��ֱ�Ӵ�������һ��С�ؿ���
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else   // ����������յ㣬��ʾ�������
        {
            levelTimer.StopTimer();
            resultCanvas.gameObject.SetActive(true);
            ShowResultCanvas();
            currentBigLevel++;
            currentSmallLevel = 1;
        }
    }

    /// <summary>
    /// С�ؿ��л�
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator FadeAndSwitchLevel()
    {
        yield return Fade(1);
        // ж�ص�ǰ����
        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene);
        // �л��ؿ�
        currentSmallLevel++;
        string nextSceneName = $"{currentBigLevel}.{currentSmallLevel}";   // GameSceneX.X ָ�ؿ��������֣��ɸ��ݺ��������������
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        yield return loadOperation; // �ȴ��������

        // �����³���Ϊ�����
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        if (newScene.isLoaded) // ȷ�������Ѽ���
        {
            SceneManager.SetActiveScene(newScene);
        }
        else
        {
            Debug.LogError($"Scene {nextSceneName} not loaded!");
            yield break;
        }
        // ���ݵ�ǰ�ؿ��������������ʼλ��
        int index = ((currentBigLevel - 1) * 2 + (currentSmallLevel - 1)) + 1;
        Debug.Log(currentSmallLevel);
        Debug.Log("����" + index);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            playerAnimator.gameObject.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
        }
        EventHandler.IdleEvent.Invoke();  // ����Ϊ����״̬
        EventHandler.isMoving = false;
        yield return Fade(0);
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

    /// <summary>
    /// ��ؿ��л�
    /// </summary>
    private void StartLoadNextBigLevel()
    {
        StartCoroutine(LoadNextBigLevel());
    }

    private System.Collections.IEnumerator LoadNextBigLevel()
    {
        yield return Fade(1);
        // ж�ص�ǰ����
        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene);

        resultCanvas.gameObject.SetActive(false);
        levelTimer.ResetTimer();
        currentSmallLevel = 1;
        string nextSceneName = $"{currentBigLevel}.{currentSmallLevel}";
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        // �����³���Ϊ�����
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        SceneManager.sceneLoaded += (Scene sc, LoadSceneMode loadSceneMode) =>
        {
            SceneManager.SetActiveScene(newScene);
        };
        // ���ݵ�ǰ�ؿ��������������ʼλ��
        int index = (currentBigLevel - 1) * 2 + (currentSmallLevel - 1);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            playerAnimator.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
        }
        EventHandler.IdleEvent.Invoke();  // ����Ϊ����״̬
        EventHandler.isMoving = false;
        yield return Fade(0);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="targetAlpha">1�Ǻڣ�0��͸��</param>
    /// <returns></returns>
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
}