using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    [SerializeField]private float elapsedTime; // ��ؿ�����ʱ��
    [SerializeField]private bool isTiming;  // �Ƿ��ڼ�ʱ״̬
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
    /// ����ʱ����ʾ
    /// </summary>
    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    /// <summary>
    /// ������ʱ
    /// </summary>
    public void StartTimer()
    {
        if (isFirstPress)  //��һ�ΰ��£���ʼ��ʱ
        {
            isTiming = true;
            isFirstPress = false;
        }
        //else
        //    Debug.Log("not isFirstPress");
    }

    /// <summary>
    /// ��ͣ��ʱ
    /// </summary>
    public void PauseTimer()
    {
        isTiming = false;
    }

    /// <summary>
    /// ������ʱ
    /// </summary>
    public void ResumeTimer()
    {
        if (!isFirstPress)
        {
            isTiming = true;
        }
    }

    /// <summary>
    /// ͨ�ش�ؿ�����¼��ǰ���õ���ʱ��
    /// </summary>
    public void StopTimer()
    {
        isTiming = false;
        // ���������Ӽ�¼ʱ����߼������籣�浽 PlayerPrefs
        // PlayerPrefs.SetFloat("LevelTime", elapsedTime);
    }

    /// <summary>
    /// ���ü�ʱ��
    /// </summary>
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isFirstPress = true;
        isTiming = false;
        UpdateTimerDisplay();
    }

    /// <summary>
    /// ���ش�ؿ���ʱ����������
    /// </summary>
    /// <returns></returns>
    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}