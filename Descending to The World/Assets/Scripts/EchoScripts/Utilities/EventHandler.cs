using UnityEngine;
using UnityEngine.Events;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetGroundEvent = new UnityEvent();

    private static bool isMoving = false;

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

    /// <summary>
    /// 触发恢复坍塌地块的事件
    /// </summary>
    //public static void CallResetGroundEvent()
    //{
    //    ResetGroundEvent.Invoke();
    //}
}
