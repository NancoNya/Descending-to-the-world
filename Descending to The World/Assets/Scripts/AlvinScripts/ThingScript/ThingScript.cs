using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThingOSType
{
    Seismograph,
    Seismograph1,
    Seismograph2,
    KongMingLantern,
    KongMingLantern1,
    KongMingLantern2,
    Sundial,
    MilkyWay,
    Compass,
    Magnet,
    Rocket,
    Rocket1
}

public class ThingScript : MonoBehaviour
{
    public ThingOSType thingOSType = ThingOSType.Seismograph;

    public void OnClick()
    {
        bool isSuccess = HandManager.instance.AddThingOS(thingOSType);
        if (isSuccess)
        {
            gameObject.SetActive(false);
        }
    }
}