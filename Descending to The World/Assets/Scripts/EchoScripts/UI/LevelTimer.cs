using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField]private float elapsedTime; // 大关卡所用时间
    [SerializeField]private bool isTiming;  // 是否处于计时状态
    private bool isFirstPress = true;

    private void Start()
    {
        elapsedTime = 0f;
        isTiming = false;
        UpdateTimerDisplay();

        EventHandler.IdleEvent.AddListener(PauseTimer);
        EventHandler.MovementEvent.AddListener(ResumeTimer);
    }

    private void OnDestroy()
    {
        EventHandler.IdleEvent.RemoveListener(PauseTimer);
        EventHandler.MovementEvent.RemoveListener(ResumeTimer);
    }

    private void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    /// <summary>
    /// 更新时间显示
    /// </summary>
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// 触发计时
    /// </summary>
    public void StartTimer()
    {
        if (isFirstPress)  //第一次按下，开始计时
        {
            isTiming = true;
            isFirstPress = false;
        }
        //else
        //    Debug.Log("not isFirstPress");
    }

    /// <summary>
    /// 暂停计时
    /// </summary>
    public void PauseTimer()
    {
        isTiming = false;
    }

    /// <summary>
    /// 继续计时
    /// </summary>
    public void ResumeTimer()
    {
        if (!isFirstPress)
        {
            isTiming = true;
        }
    }

    /// <summary>
    /// 通关大关卡，记录当前所用的总时间
    /// </summary>
    public void StopTimer()
    {
        isTiming = false;
        // 这里可以添加记录时间的逻辑，例如保存到 PlayerPrefs
        // PlayerPrefs.SetFloat("LevelTime", elapsedTime);
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isFirstPress = true;
        isTiming = false;
        UpdateTimerDisplay();
    }

    /// <summary>
    /// 返回大关卡总时间给结算界面
    /// </summary>
    /// <returns></returns>
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}