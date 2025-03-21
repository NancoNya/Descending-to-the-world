using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingPickUp : MonoBehaviour
{

    public ThingPickUpType thingPickUpType = ThingPickUpType.Seismometer;

    private Ray ray;
    private RaycastHit hit;
    public ThingPickUpType currentThingPickUpType;
    public void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                bool isSuccess = HandManager.instance.AddThingOS(thingPickUpType);
                Destroy(gameObject);
            }
        }
    }
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
            //            hit.t = thingOnScene.gameObject;
            //            //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //            //mouseWorldPosition.z = 0;
            //            thingOSType = cell.currentThing;
            //            //cell.currentThing.transform.position = mouseWorldPosition;
            //            //Time.timeScale = 0;
            //            cell.currentThing = null;
            //            HandManager.instance.AddThingOS(ThingPickUpType);
            //            Destroy(this);
            //        }
            //    }
            //}
       
