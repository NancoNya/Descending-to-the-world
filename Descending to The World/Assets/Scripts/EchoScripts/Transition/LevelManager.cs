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
    [Header("计时器")]
    public LevelTimer levelTimer;

    [Header("人物动画")]
    private Animator playerAnimator;

    [Header("小关卡切换的渐隐渐出")]
    public CanvasGroup fadeCanvasGroup;  // 调整alpha值实现遮挡
    public float fadeDuration;  // 渐隐渐出的持续时间
    [SerializeField]private bool isFading;

    [Header("大小关卡切换")]
    [SerializeField] private int currentBigLevel = 1;   // 所处大关卡
    [SerializeField] private int currentSmallLevel = 1;   // 所处小关卡
    public Button nextLevelButton;   // 通关大关卡，点击前往下一个大关卡
    public TextMeshProUGUI resultTimeText;   // 显示大关卡通关时间
    public Canvas resultCanvas;   // 结算界面（通关大关卡时出现）
    public LevelInitialSO levelInitialData;  // 人物进入小场景的初始数据（位置）


    private void Start()
    {
        resultCanvas.gameObject.SetActive(false);
        nextLevelButton.onClick.AddListener(StartLoadNextBigLevel);

        // 获取人物动画
        if (GameObject.FindWithTag("Player"))
            playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        else
            Debug.Log("didn't find PlayerAnimator");
    }

    /// <summary>
    /// 到达小关卡终点
    /// </summary>
    public void OnReachCheckpoint()
    {
        levelTimer.PauseTimer();
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)   // 停止人物移动，播放鼓掌动画
        {
            player.SetHorizontalVelocityZero();
            player.ClapAnim();
        }
        else
            Debug.Log("did't find PlayerController");

        if (currentSmallLevel < 3 && !isFading)   // 到达第一或第二个小关卡的终点，且不处于fade状态，直接传送至下一个小关卡。
        {
            StartCoroutine(FadeAndSwitchLevel());
        }
        else   // 到达第三个终点，显示结算界面
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
        string nextSceneName = $"GameScene{currentBigLevel}.{currentSmallLevel}";   // GameSceneX.X 指关卡场景名字，可根据后期命名需求更改
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
        // 根据当前关卡索引设置人物初始位置
        int index = ((currentBigLevel - 1) * 3 + (currentSmallLevel - 1)) + 1;
        Debug.Log(currentSmallLevel);
        Debug.Log("索引" + index);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            playerAnimator.gameObject.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
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
        yield return SceneManager.UnloadSceneAsync(currentScene);

        resultCanvas.gameObject.SetActive(false);
        levelTimer.ResetTimer();
        currentSmallLevel = 1;
        string nextSceneName = $"GameScene{currentBigLevel}.{currentSmallLevel}";
        SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
        // 设置新场景为激活场景
        Scene newScene = SceneManager.GetSceneByName(nextSceneName);
        SceneManager.sceneLoaded += (Scene sc, LoadSceneMode loadSceneMode) =>
        {
            SceneManager.SetActiveScene(newScene);
        };
        // 根据当前关卡索引设置人物初始位置
        int index = (currentBigLevel - 1) * 3 + (currentSmallLevel - 1);
        if (levelInitialData != null && index < levelInitialData.playerPositions.Length)
        {
            playerAnimator.transform.position = levelInitialData.playerPositions[index];
            playerAnimator.gameObject.SetActive(true);
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