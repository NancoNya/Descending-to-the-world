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
    [Header("计时器")]
    public LevelTimer levelTimer;

    [Header("小关卡切换 渐隐渐出")]
    public float fadeDuration = 0.5f;  // 渐隐渐出的持续时间
    [SerializeField]private bool isFading;

    [Header("大小关卡 切换数据")]
    public int currentBigLevel = 1;   // 所处大关卡
    public int currentSmallLevel = 1;   // 所处小关卡
    public LevelInitialSO levelInitialData;  // 人物进入小场景的初始数据（位置）
    [SerializeField]private GameObject player;

    [Header("动态UI引用")]
    public GameObject canvasFather;
    public Canvas resultCanvas;   // 结算界面（通关大关卡时出现）
    public CanvasGroup fadeCanvasGroup;  // 调整alpha值实现遮挡
    public Canvas gameCanvas;

    public Button nextLevelButton;    // 通关大关卡，点击前往下一个大关卡
    public TextMeshProUGUI resultTimeText;   // 显示大关卡通关时间

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

    // 用于通知 PlayerControllerScript 更新位置
    public static event System.Action SceneSwitchedEvent;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // player = FindPlayerInScene();
        FindChildObjects();
        SetInitialStates();
        if (mode == LoadSceneMode.Additive)
        {
            // 遍历所有加载的场景
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene loadedScene = SceneManager.GetSceneAt(i);
                if (loadedScene.name == "PropColumn")   // 检查是否加载了 PropColumn 场景
                {
                    // 在 PropColumn 场景中查找 Player 物体
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
            nextLevelButton.onClick.RemoveAllListeners(); // 移除之前的所有监听器
            nextLevelButton.onClick.AddListener(StartLoadNextBigLevel);
        }
    }

    /// <summary>
    /// 获取canvas子物体
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
        //    Debug.Log("未获取canvas子物体");

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
    /// result canvas初始化 (非激活状态)
    /// </summary>
    private void SetInitialStates()
    {
        if (resultCanvas != null)
            resultCanvas.gameObject.SetActive(false);
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.gameObject.SetActive(false);
    }

    /// <summary>
    /// 到达关卡终点
    /// </summary>
    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();

        if (currentSmallLevel < 2 && !isFading)   // 到达第一或第二个小关卡的终点，且不处于fade状态，直接传送至下一个小关卡。
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else   // 到达第二个终点，显示结算界面
        {
            levelTimer.StopTimer();
            resultCanvas.gameObject.SetActive(true);
            ShowResultCanvas();
            currentBigLevel++;
            currentSmallLevel = 1;
        }
    }

    /// <summary>
    /// 小关卡切换
    /// </summary>
    /// <returns></returns>
    private System.Collections.IEnumerator FadeAndSwitchLevel()
    {
        yield return Fade(1);
        // 卸载当前场景
        Scene currentScene = SceneManager.GetActiveScene();
        yield return SceneManager.UnloadSceneAsync(currentScene);
        // 切换关卡
        currentSmallLevel++;
        string nextSceneName = $"{currentBigLevel}.{currentSmallLevel}";   // GameSceneX.X 指关卡场景名字，可根据后期命名需求更改
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        yield return loadOperation; // 等待加载完成

        // 设置新场景为激活场景
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        if (newScene.isLoaded) // 确保场景已加载
        {
            SceneManager.SetActiveScene(newScene);
        }
        else
        {
            Debug.LogError($"Scene {nextSceneName} not loaded!");
            yield break;
        }
        
        EventHandler.IdleEvent.Invoke();  // 设置为待机状态
        EventHandler.isMoving = false;
        yield return Fade(0);
    }

    /// <summary>
    /// 通关大关卡，显示结算界面
    /// </summary>
    private void ShowResultCanvas()
    {
        resultCanvas.gameObject.SetActive(true);
        
        float totalTime = levelTimer.GetElapsedTime();
        int minutes = Mathf.FloorToInt(totalTime / 60);
        int seconds = Mathf.FloorToInt(totalTime % 60);
        resultTimeText.text = string.Format("通关时间: {0:00}:{1:00}", minutes, seconds);
        // 可以在这里添加保存时间等结算逻辑
    }

    /// <summary>
    /// 大关卡切换
    /// </summary>
    private void StartLoadNextBigLevel()
    {
        StartCoroutine(LoadNextBigLevel());
    }

    private System.Collections.IEnumerator LoadNextBigLevel()
    {
        yield return Fade(1);
        // 卸载当前场景
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
        yield return mainLoadOperation; // 等待主场景加载完成

        // 以 Additive 模式加载名为 "PropColumn" 的场景
        AsyncOperation propLoadOperation = SceneManager.LoadSceneAsync("PropColumn", LoadSceneMode.Additive);
        yield return propLoadOperation; // 等待 PropColumn 场景加载完成

        // 设置新场景为激活场景
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        Debug.Log(newScene.name);
        SceneManager.SetActiveScene(newScene);
        //SceneManager.sceneLoaded += (Scene sc, LoadSceneMode loadSceneMode) =>
        //{
        //    SceneManager.SetActiveScene(newScene);
        //};
        // 根据当前关卡索引设置人物初始位置
        int index = (currentBigLevel - 1) * 2 + (currentSmallLevel - 1);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            player.transform.position = levelInitialData.playerPositions[index];
            player.gameObject.SetActive(true);
        }
        EventHandler.IdleEvent.Invoke();  // 设置为待机状态
        EventHandler.isMoving = false;
        yield return Fade(0);
    }

    /// <summary>
    /// 渐隐渐出
    /// </summary>
    /// <param name="targetAlpha">1是黑，0是透明</param>
    /// <returns></returns>
    private IEnumerator Fade(float targetAlpha)
    {
        isFading = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;

        //持续更新透明度，不断调用Mathf.MoveTowards方法
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }
        isFading = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }
}