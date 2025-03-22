using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThingScript;


public class ThingOnScene : MonoBehaviour
{
    public ThingOSType thingOSType = ThingOSType.Seismometer;
    private bool hasBeenPickedUp = false;

    public void OnClick()
    {
        if (HandManager.instance == null)
        {
            Debug.LogError("HandManager ����ʵ��δ��ʼ����");
            return;
        }

        if (hasBeenPickedUp)
        {
            Debug.Log($"�ٴε���� {thingOSType} ���͵ĵ��ߣ����� HandManager ����");
            HandManager.instance.HandleDoubleClickThing(thingOSType, gameObject);
        }
        else
        {
            Debug.Log($"�״ε���� {thingOSType} ���͵ĵ��ߣ�������ӵ�����");
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
            Debug.LogError("HandManager ����ʵ��δ��ʼ����");
            return;
        }
        Debug.Log($"�ڵ�Ԫ���е���� {thingOSType} ���͵ĵ���");
        Cell cell = GetComponentInParent<Cell>();
        if (cell == null)
        {
            Debug.LogError("δ�ҵ�����Ԫ�����");
            return;
        }
        Debug.Log("׼������ HandManager �� PickUpThingFromCell ����");
        HandManager.instance.PickUpThingFromCell(cell);
        StartCoroutine(DestroyAfterPickup());
    }

    private IEnumerator DestroyAfterPickup()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"���ٵ���: {thingOSType}");
        HandManager.instance.MarkThingAsDestroyed(thingOSType);
        Destroy(gameObject);
    }
}