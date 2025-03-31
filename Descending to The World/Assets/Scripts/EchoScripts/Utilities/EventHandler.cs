using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetEvent = new UnityEvent();   // 坍塌地块，彩云，道具恢复原位
    public static UnityEvent TimerStopEvent = new UnityEvent();
    public static bool isMoving = false;

    /// <summary>
    /// 点击时钟触发移动或回溯
    /// </summary>
    public static void CallGameControlButtonClick()
    {
        if (isMoving)   // 点击后回溯
        {
            Debug.Log("回到出发点");
            IdleEvent.Invoke();
            ResetEvent.Invoke();
        }
        else   // 点击后开始移动
        {
            Debug.Log("开始移动");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }

    /// <summary>
    /// 人物掉落至场景外：回溯，计时器暂停
    /// </summary>
    public static void CallTimerStopEvent()
    {
        LevelManager levelManager = LevelManager.Instance;
        levelManager.levelTimer.PauseTimer();
    }
}
