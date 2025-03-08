using UnityEngine;
using UnityEngine.Events;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();

    private static bool isMoving = false;

    /// <summary>
    /// 点击时钟触发移动或回溯
    /// </summary>
    public static void OnGameControlButtonClick()
    {
        if (isMoving)
        {
            Debug.Log("回到出发点");
            IdleEvent.Invoke();
        }
        else
        {
            Debug.Log("开始移动");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }
}
