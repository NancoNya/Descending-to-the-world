using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetGroundEvent = new UnityEvent();

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

    public static event System.Action SceneSwitchedEvent;

    ///// <summary>
    ///// 用于通知 PlayerControllerScript 更新位置
    ///// </summary>
    ///// <param name="scene"></param>
    ///// <param name="mode"></param>
    //public static void CallSceneSwitchedEvent(Scene scene, LoadSceneMode mode)
    //{
    //    if (mode == LoadSceneMode.Additive)
    //    {
    //        SceneSwitchedEvent?.Invoke();
    //    }
    //}
}
