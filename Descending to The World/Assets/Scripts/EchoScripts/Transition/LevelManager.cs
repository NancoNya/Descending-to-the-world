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

    [Header("С�ؿ��л� ��������")]
    public float fadeDuration;  // ���������ĳ���ʱ��
    [SerializeField]private bool isFading;

    [Header("��С�ؿ� �л�����")]
    [SerializeField] private int currentBigLevel = 1;   // ������ؿ�
    [SerializeField] private int currentSmallLevel = 1;   // ����С�ؿ�
    public LevelInitialSO levelInitialData;  // �������С�����ĳ�ʼ���ݣ�λ�ã�
    //public Button nextLevelButton;   // ͨ�ش�ؿ������ǰ����һ����ؿ�
    //public TextMeshProUGUI resultTimeText;   // ��ʾ��ؿ�ͨ��ʱ��
    //public Canvas resultCanvas;   // ������棨ͨ�ش�ؿ�ʱ���֣�

    [Header("��̬UI����")]
    public GameObject canvasFather;

    public Canvas resultCanvas;   // ������棨ͨ�ش�ؿ�ʱ���֣�
    public CanvasGroup fadeCanvasGroup;  // ����alphaֵʵ���ڵ�
    public Canvas gameCanvas;

    public Button nextLevelButton;    // ͨ�ش�ؿ������ǰ����һ����ؿ�
    public TextMeshProUGUI resultTimeText;   // ��ʾ��ؿ�ͨ��ʱ��
    // public GameObject pauseMenu;

    private void Start()
    {
        // resultCanvas.gameObject.SetActive(false);
        nextLevelButton.onClick.AddListener(StartLoadNextBigLevel);

        // ��ȡ���ﶯ��
        if (GameObject.FindWithTag("Player"))
        {
            playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
            Debug.Log("player has been found");
        }
        else
            Debug.Log("didn't find PlayerAnimator");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindChildObjects();
        SetInitialStates();
        /////// ��ȡresult canvas
        //GameObject canvasGameObject = GameObject.Find("CanvasName");
        //if (canvasGameObject != null)
        //{ 
        //    resultCanvas = canvasGameObject.GetComponent<Canvas>();
        //}
        //if (this.resultCanvas != null)
        //{
        //    // ���������岢��ֵ
        //    nextLevelButton = this.resultCanvas.transform.Find("NextLevelButton").GetComponent<Button>();
        //    resultTimeText = this.resultCanvas.transform.Find("ResultTimeText").GetComponent<TextMeshProUGUI>();
        //}
        //else
        //    Debug.Log("δ�ҵ�resultcanvas");
        /////// ��ȡfade canvasgroup
        //GameObject fadeCanvasGroupObj = GameObject.Find("Fade Canvas");
        //if(fadeCanvasGroupObj != null)
        //{
        //    fadeCanvasGroup = fadeCanvasGroupObj.GetComponent<CanvasGroup>();
        //}
    }

    private void FindChildObjects()
    {
        canvasFather = GameObject.Find("CanvasFather");
        if (canvasFather != null)
        {
            resultCanvas = canvasFather.transform.Find("Result Canvas")?.GetComponent<Canvas>();
            fadeCanvasGroup = canvasFather.transform.Find("Fade Canvas")?.transform.Find("Panel").GetComponent<CanvasGroup>();
            gameCanvas = canvasFather.transform.Find("Game Canvas")?.GetComponent<Canvas>();
        }
        else
            Debug.Log("δ��ȡcanvas������");

        if(gameCanvas != null)
        {
            levelTimer = gameCanvas.transform.GetChild(2).GetComponent<LevelTimer>();
        }
    }

    private void SetInitialStates()
    {
        if (resultCanvas != null)
            resultCanvas.gameObject.SetActive(false);
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����С�ؿ��յ�
    /// </summary>
    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();
        
        //PlayerController player = FindObjectOfType<PlayerController>();
        //if (player != null)   // ֹͣ�����ƶ������Ź��ƶ���
        //{
        //    player.SetHorizontalVelocityZero();
        //    //player.ClapAnim();
        //}
        //else
        //    Debug.Log("did't find PlayerController");

        if (currentSmallLevel < 2 && !isFading)   // �����һ��ڶ���С�ؿ����յ㣬�Ҳ�����fade״̬��ֱ�Ӵ�������һ��С�ؿ���
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else   // ����ڶ����յ㣬��ʾ�������
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
        Debug.Log("����" + index);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            Debug.Log("aaa");
            playerAnimator.gameObject.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
            Debug.Log("bbb");
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
            Debug.Log("aaa");
            playerAnimator.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
            Debug.Log("bbb");
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