using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetGroundEvent = new UnityEvent();
    public static UnityEvent TimerStopEvent = new UnityEvent();

    public static bool isMoving = false;

    /// <summary>
    /// 点击时钟触发移动或回溯
    /// </summary>
    public static void CallGameControlButtonClick()
    {
        if (isMoving)
        {
            Debug.Log("回到出发点");
            IdleEvent.Invoke();
            ResetGroundEvent.Invoke();
        }
        else
        {
            Debug.Log("开始移动");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }

    public static void CallTimerStopEvent()
    {
        LevelManager levelManager = LevelManager.Instance;
        levelManager.levelTimer.PauseTimer();
    }
}
