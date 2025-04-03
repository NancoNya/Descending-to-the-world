using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;
// using UnityEditor.SearchService;
using Scene = UnityEngine.SceneManagement.Scene;

public class LevelManager : Singleton<LevelManager>
{
    [Header("��ʱ��")]
    public LevelTimer levelTimer;

    [Header("С�ؿ��л� ��������")]
    public float fadeDuration = 0.5f;  // ���������ĳ���ʱ��
    [SerializeField]private bool isFading;

    [Header("��С�ؿ� �л�����")]
    public int currentBigLevel = 1;   // ������ؿ�
    public int currentSmallLevel = 1;   // ����С�ؿ�
    public LevelInitialSO levelInitialData;  // �������С�����ĳ�ʼ���ݣ�λ�ã�
    [SerializeField]private GameObject player;

    [Header("��̬UI����")]
    public GameObject canvasFather;
    public Canvas resultCanvas;   // ������棨ͨ�ش�ؿ�ʱ���֣�
    public CanvasGroup fadeCanvasGroup;  // ����alphaֵʵ���ڵ�
    public Canvas gameCanvas;

    public Button nextLevelButton;    // ͨ�ش�ؿ������ǰ����һ����ؿ�
    public TextMeshProUGUI resultTimeText;   // ��ʾ��ؿ�ͨ��ʱ��

    //override protected void Awake()
    //{
    //    levelInitialData = Resources.Load<LevelInitialSO>("LevelInitialSO");
    //}

    private void Start()
    {
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ����֪ͨ PlayerControllerScript ����λ��
    public static event System.Action SceneSwitchedEvent;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // player = FindPlayerInScene();
        FindChildObjects();
        SetInitialStates();
        if (mode == LoadSceneMode.Additive)
        {
            // �������м��صĳ���
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == "PropColumn")   // ����Ƿ������ PropColumn ����
                {
                    // �� PropColumn �����в��� Player ����
                    GameObject[] rootObjects = loadedScene.GetRootGameObjects();
                    foreach (GameObject rootObject in rootObjects)
                    {
                        Transform playerTransform = rootObject.transform.Find("///Player");
                        if (playerTransform != null)
                        {
                            player = playerTransform.gameObject;
                            SceneSwitchedEvent?.Invoke();
                            break;
                        }
                    }
                    break;
                }
            }
        }
        if (nextLevelButton != null)
        {
            nextLevelButton.onClick.RemoveAllListeners(); // �Ƴ�֮ǰ�����м�����
            nextLevelButton.onClick.AddListener(StartLoadNextBigLevel);
        }
    }

    /// <summary>
    /// ��ȡcanvas������
    /// </summary>
    private void FindChildObjects()
    {
        canvasFather = GameObject.Find("CanvasFather");
        if (canvasFather != null)
        {
            resultCanvas = canvasFather.transform.Find("Result Canvas")?.GetComponent<Canvas>();
            fadeCanvasGroup = canvasFather.transform.Find("Fade Canvas")?.transform.Find("Panel").GetComponent<CanvasGroup>();
            gameCanvas = canvasFather.transform.Find("Game Canvas")?.GetComponent<Canvas>();
        }
        //else
        //    Debug.Log("δ��ȡcanvas������");

        if(gameCanvas != null)
        {
            levelTimer = gameCanvas.transform.GetChild(1).GetComponent<LevelTimer>();
        }
        if(resultCanvas != null)
        {
            resultTimeText = resultCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            nextLevelButton = resultCanvas.transform.GetChild(2).transform.GetChild(0).GetComponent<Button>();
        }
        
    }

    /// <summary>
    /// result canvas��ʼ�� (�Ǽ���״̬)
    /// </summary>
    private void SetInitialStates()
    {
        if (resultCanvas != null)
            resultCanvas.gameObject.SetActive(false);
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.gameObject.SetActive(false);
    }

    /// <summary>
    /// ����ؿ��յ�
    /// </summary>
    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();

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
        //Scene propScene = SceneManager.GetSceneByName()
        yield return SceneManager.UnloadSceneAsync(currentScene);

        yield return SceneManager.UnloadSceneAsync("PropColumn");

        resultCanvas.gameObject.SetActive(false);
        levelTimer.ResetTimer();
        currentSmallLevel = 1;
        string nextSceneName = $"{currentBigLevel}.{currentSmallLevel}";
        // SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

        AsyncOperation mainLoadOperation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        yield return mainLoadOperation; // �ȴ��������������

        // �� Additive ģʽ������Ϊ "PropColumn" �ĳ���
        AsyncOperation propLoadOperation = SceneManager.LoadSceneAsync("PropColumn", LoadSceneMode.Additive);
        yield return propLoadOperation; // �ȴ� PropColumn �����������

        // �����³���Ϊ�����
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        Debug.Log(newScene.name);
        SceneManager.SetActiveScene(newScene);
        //SceneManager.sceneLoaded += (Scene sc, LoadSceneMode loadSceneMode) =>
        //{
        //    SceneManager.SetActiveScene(newScene);
        //};
        // ���ݵ�ǰ�ؿ��������������ʼλ��
        int index = (currentBigLevel - 1) * 2 + (currentSmallLevel - 1);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            player.transform.position = levelInitialData.playerPositions[index];
            player.gameObject.SetActive(true);
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