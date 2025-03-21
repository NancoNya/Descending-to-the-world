using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static Cell instance { get; private set; }
    public ThingOnScene currentThing;
    public ThingPickUp currentPickUp;
    //private int timer = 0;
    //public string Name;
    private void OnMouseDown()
    {
        //timer %= 2;
        //if (timer == 0)
        //{
            HandManager.instance.OnCellClick(this);
        //}
        //else if (timer == 1) { HandManager.instance.OnCellClickAgain(this); }
    }

    public bool AddThingOS(ThingOnScene thingOnScene)
    {
        if (currentThing != null) return false;
        currentThing = thingOnScene;
        currentThing.transform.position = transform.position;
        return true;
    }

    public bool AddThingOS(ThingPickUp thingPickUp)
    {
        if (currentPickUp !=null) return false;
        currentPickUp = thingPickUp;
        currentPickUp.transform.position = transform.position;
        return true;
    }

    //public bool MoveThingOS(ThingOnScene thingOnScene)
    //{

    //    //if (currentThing != null) return false;
    //    //currentThing = thingOnScene;
    //    //currentThing.transform.position = transform.position;
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
    //        {
    //            Name = hit.collider.gameObject.name;
    //            Debug.Log(Name);
    //        }
    //    }
    //    currentThing = null;
    //    return true;
    //}

}
