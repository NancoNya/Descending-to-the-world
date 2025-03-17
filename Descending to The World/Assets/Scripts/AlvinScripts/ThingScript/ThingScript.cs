using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ThingType
{
    Seismometer,
    KongMingLantern,
    Sundial,
    MilkyWay,
    Compass,
    Magnet
}


public class ThingScript : MonoBehaviour
{
    public ThingType thingType = ThingType.Seismometer;
    public ThingOSType thingOSType = ThingOSType.Seismometer;

    public void OnClick()
    {
        //Destroy(gameObject);
        bool isSuccess = HandManager.instance.AddThingOS(thingOSType);

    }

}
