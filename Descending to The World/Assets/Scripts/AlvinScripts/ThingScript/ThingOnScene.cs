using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ThingScript;

public enum ThingPickUpType
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
public class ThingOnScene : MonoBehaviour
{

    public ThingOSType thingOSType = ThingOSType.Seismometer;

    public void OnClick()
    {
        HandManager.instance.AddThingOS(thingOSType);
    }


    //public void OnClickAgain()
    //{
    //    Cell cell = GetComponent<Cell>();
    //    HandManager handManager = GetComponent<HandManager>();
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        RaycastHit hit;
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        if (Physics.Raycast(ray, out hit))
    //        {

    //            //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //            //mouseWorldPosition.z = 0;
    //            HandManager.instance.currentThing = cell.currentThing;
    //            //cell.currentThing.transform.position = mouseWorldPosition;
    //            //Time.timeScale = 0;
    //            cell.currentThing = null;
    //            HandManager.instance.AddThingOS(thingOSType);
    //            Destroy(this);
    //        }
    //    }
    //    //public void OnClickAgain()
    //    //{
    //    //    HandManager.instance.MoveThingOS(thingOSType);
    //    //}

    //}
}
