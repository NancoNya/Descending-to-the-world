using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThingScript;

public enum ThingOSType
{
    Seismometer,
    KongMingLantern
}
public class ThingOnScene : MonoBehaviour
{


    public ThingOSType thingOSType = ThingOSType.Seismometer;

    public void OnClick()
    {
        Debug.Log("���ö������͵�Ԥ����ʵ����������");
        HandManager.instance.AddThingOS(thingOSType);
    }
}
