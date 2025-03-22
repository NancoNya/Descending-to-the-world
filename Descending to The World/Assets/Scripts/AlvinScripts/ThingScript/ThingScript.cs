using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThingOSType
{
    Seismograph,
    Seismometer,
    KongMingLantern,
    Sundial,
    MilkyWay,
    Compass,
    Magnet,
    Rocket
}

public class ThingScript : MonoBehaviour
{
    public ThingOSType thingOSType = ThingOSType.Seismometer;

    public void OnClick()
    {
        bool isSuccess = HandManager.instance.AddThingOS(thingOSType);
        if (isSuccess)
        {
            gameObject.SetActive(false);
        }
    }
}