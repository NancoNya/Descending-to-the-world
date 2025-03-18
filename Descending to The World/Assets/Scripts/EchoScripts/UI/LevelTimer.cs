using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField]private float elapsedTime; // ����ʱ��
    [SerializeField]private bool isTiming;  // ��ʱ״̬
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
    /// ������ʱ
    /// </summary>
    public void StartTimer()
    {
        //��һ�ΰ��£���ʼ��ʱ
        if (isFirstPress)
        {
            isTiming = true;
            isFirstPress = false;
        }
        //������ʱ
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
    /// ͨ�ش�ؿ�����¼��ǰ���õ���ʱ��
    /// </summary>
    public void StopTimer()
    {
        isTiming = false;
        // ���������Ӽ�¼ʱ����߼������籣�浽 PlayerPrefs
        PlayerPrefs.SetFloat("LevelTime", elapsedTime);
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}