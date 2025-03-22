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
            Debug.Log("单元格中有道具，尝试拾取");
            currentThing.OnClickInCell();
        }
        else
        {
            Debug.Log("单元格中没有道具，尝试放置手中道具");
            if (HandManager.instance == null)
            {
                Debug.LogError("HandManager 单例实例未初始化！");
                return;
            }
            HandManager.instance.OnCellClick(this);
        }
    }

    public bool AddThingOS(ThingOnScene thingOnScene)
    {
        if (currentThing != null)
        {
            Debug.Log("单元格中已有道具，无法添加");
            return false;
        }
        currentThing = thingOnScene;
        currentThing.transform.position = transform.position;
        // 设置道具的父对象为当前单元格对象
        currentThing.transform.SetParent(transform);
        Debug.Log($"成功将 {thingOnScene.thingOSType} 类型的道具添加到单元格");
        return true;
    }
}