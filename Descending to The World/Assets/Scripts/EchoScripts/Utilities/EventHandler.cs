using UnityEngine;
using UnityEngine.Events;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();
    public static UnityEvent ResetGroundEvent = new UnityEvent();

    private static bool isMoving = false;

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

    /// <summary>
    /// �����ָ�̮���ؿ���¼�
    /// </summary>
    //public static void CallResetGroundEvent()
    //{
    //    ResetGroundEvent.Invoke();
    //}
}
