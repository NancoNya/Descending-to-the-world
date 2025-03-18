using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField]private float elapsedTime; // 所用时间
    [SerializeField]private bool isTiming;  // 计时状态
    private bool isFirstPress = true;

    private void Start()
    {
        elapsedTime = 0f;
        isTiming = false;
        timerText.text = "0:00";

        EventHandler.IdleEvent.AddListener(PauseTimer);
        EventHandler.MovementEvent.AddListener(ResumeTimer);
    }

    private void OnDestroy()
    {
        EventHandler.IdleEvent.RemoveListener(PauseTimer);
        EventHandler.MovementEvent.RemoveListener(ResumeTimer);
    }

    /// <summary>
    /// 触发计时
    /// </summary>
    public void StartTimer()
    {
        //第一次按下，开始计时
        if (isFirstPress)
        {
            isTiming = true;
            isFirstPress = false;
        }
        //继续计时
        else
        {
            isTiming = !isTiming;
        }
    }

    public void PauseTimer()
    {
        isTiming = false;
    }

    public void ResumeTimer()
    {
        if (!isFirstPress)
        {
            isTiming = true;
        }
    }

    private void Update()
    {
        if (isTiming)
        {
            elapsedTime += Time.deltaTime;
            DisplayTime(elapsedTime);
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// 通关大关卡，记录当前所用的总时间
    /// </summary>
    public void StopTimer()
    {
        isTiming = false;
        // 这里可以添加记录时间的逻辑，例如保存到 PlayerPrefs
        PlayerPrefs.SetFloat("LevelTime", elapsedTime);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}