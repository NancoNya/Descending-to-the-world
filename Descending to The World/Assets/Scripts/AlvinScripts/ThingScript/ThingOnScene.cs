using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThingScript;


public class ThingOnScene : MonoBehaviour
{
    public ThingOSType thingOSType = ThingOSType.Seismograph;
    private bool hasBeenPickedUp = false;

    public void OnClick()
    {
        if (HandManager.instance == null)
        {
            Debug.LogError("HandManager 单例实例未初始化！");
            return;
        }

        if (hasBeenPickedUp)
        {
            Debug.Log($"再次点击了 {thingOSType} 类型的道具，调用 HandManager 处理");
            HandManager.instance.HandleDoubleClickThing(thingOSType, gameObject);
        }
        else
        {
            Debug.Log($"首次点击了 {thingOSType} 类型的道具，尝试添加到手中");
            bool added = HandManager.instance.AddThingOS(thingOSType);
            if (added)
            {
                hasBeenPickedUp = true;
                gameObject.SetActive(false);
            }
        }
    }

    public void OnClickInCell()
    {
        if (HandManager.instance == null)
        {
            Debug.LogError("HandManager 单例实例未初始化！");
            return;
        }
        Debug.Log($"在单元格中点击了 {thingOSType} 类型的道具");
        Cell cell = GetComponentInParent<Cell>();
        if (cell == null)
        {
            Debug.LogError("未找到父单元格对象");
            return;
        }
        Debug.Log("准备调用 HandManager 的 PickUpThingFromCell 方法");
        HandManager.instance.PickUpThingFromCell(cell);
        StartCoroutine(DestroyAfterPickup());
    }

    private IEnumerator DestroyAfterPickup()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"销毁道具: {thingOSType}");
        HandManager.instance.MarkThingAsDestroyed(thingOSType);
        Destroy(gameObject);
    }
}