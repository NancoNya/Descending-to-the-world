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
        Debug.Log("将该对象类型的预制体实例化到手中");
        HandManager.instance.AddThingOS(thingOSType);
    }
}
