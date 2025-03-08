using UnityEngine;
using UnityEngine.Events;

public static class EventHandler
{
    public static UnityEvent MovementEvent = new UnityEvent();
    public static UnityEvent IdleEvent = new UnityEvent();

    private static bool isMoving = false;

    /// <summary>
    /// ���ʱ�Ӵ����ƶ������
    /// </summary>
    public static void OnGameControlButtonClick()
    {
        if (isMoving)
        {
            Debug.Log("�ص�������");
            IdleEvent.Invoke();
        }
        else
        {
            Debug.Log("��ʼ�ƶ�");
            MovementEvent.Invoke();
        }
        isMoving = !isMoving;
    }
}
