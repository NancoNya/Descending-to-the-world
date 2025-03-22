using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static Cell instance { get; private set; }
    public ThingOnScene currentThing;

    private void OnMouseDown()
    {
        if (currentThing != null)
        {
            Debug.Log("��Ԫ�����е��ߣ�����ʰȡ");
            currentThing.OnClickInCell();
        }
        else
        {
            Debug.Log("��Ԫ����û�е��ߣ����Է������е���");
            if (HandManager.instance == null)
            {
                Debug.LogError("HandManager ����ʵ��δ��ʼ����");
                return;
            }
            HandManager.instance.OnCellClick(this);
        }
    }

    public bool AddThingOS(ThingOnScene thingOnScene)
    {
        if (currentThing != null)
        {
            Debug.Log("��Ԫ�������е��ߣ��޷����");
            return false;
        }
        currentThing = thingOnScene;
        currentThing.transform.position = transform.position;
        // ���õ��ߵĸ�����Ϊ��ǰ��Ԫ�����
        currentThing.transform.SetParent(transform);
        Debug.Log($"�ɹ��� {thingOnScene.thingOSType} ���͵ĵ�����ӵ���Ԫ��");
        return true;
    }
}