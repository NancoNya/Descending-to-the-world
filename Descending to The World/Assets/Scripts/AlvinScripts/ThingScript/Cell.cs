using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public ThingOnScene currentThing;

    private void OnMouseDown()
    {
        print("111");
        HandManager.instance.OnCellClick(this);

    }
    public bool AddThingOS(ThingOnScene thingOnScene)
    {
        if (currentThing != null) return false;
        currentThing = thingOnScene;
        currentThing.transform.position = transform.position;
        return true;
    }
}
