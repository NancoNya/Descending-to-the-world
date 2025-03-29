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
    /// ���ʱ�Ӵ����ƶ������
    /// </summary>
    public static void CallGameControlButtonClick()
    {
        if (isMoving)
        {
            Debug.Log("�ص�������");
            IdleEvent.Invoke();
            ResetGroundEvent.Invoke();
        }
        else
        {
            Debug.Log("��ʼ�ƶ�");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }

    public static event System.Action SceneSwitchedEvent;

    ///// <summary>
    ///// ����֪ͨ PlayerControllerScript ����λ��
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
