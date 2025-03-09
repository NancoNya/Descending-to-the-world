using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThingScript;

public enum ThingOSType
{
    Seismometer
}
public class ThingOnScene : MonoBehaviour
{


    public ThingOSType thingOSType = ThingOSType.Seismometer;

    public void OnClick()
    {
        HandManager.instance.AddThingOS(thingOSType);
    }
}
