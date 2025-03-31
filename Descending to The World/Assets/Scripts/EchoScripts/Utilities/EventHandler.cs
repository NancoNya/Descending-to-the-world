using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetEvent = new UnityEvent();   // ̮���ؿ飬���ƣ����߻ָ�ԭλ
    public static UnityEvent TimerStopEvent = new UnityEvent();
    public static bool isMoving = false;

    /// <summary>
    /// ���ʱ�Ӵ����ƶ������
    /// </summary>
    public static void CallGameControlButtonClick()
    {
        if (isMoving)   // ��������
        {
            Debug.Log("�ص�������");
            IdleEvent.Invoke();
            ResetEvent.Invoke();
        }
        else   // �����ʼ�ƶ�
        {
            Debug.Log("��ʼ�ƶ�");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }

    /// <summary>
    /// ��������������⣺���ݣ���ʱ����ͣ
    /// </summary>
    public static void CallTimerStopEvent()
    {
        LevelManager levelManager = LevelManager.Instance;
        levelManager.levelTimer.PauseTimer();
    }
}
