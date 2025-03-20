using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance { get; private set; }

    public List<ThingOnScene> ThingOnSceneList;
    private ThingOnScene currentThing;
    private void Awake()
    {
        instance = this;
    }

    public bool AddThingOS(ThingOSType thingOSType)
    {
        if (currentThing != null) return false;
        ThingOnScene thingOSPrefab = GetThingOSPrefab(thingOSType);
        if (thingOSPrefab == null) {print("Null");return false;}
        currentThing = GameObject.Instantiate(thingOSPrefab);
        return true;
    }
    

    private ThingOnScene GetThingOSPrefab(ThingOSType thingOSType)
    {
        foreach(ThingOnScene thingOnScene in ThingOnSceneList)
        if(thingOnScene.thingOSType == thingOSType)return thingOnScene;
        return null;
    }

    // Update is called once per frame
void FollowCursor()
    {
        if (currentThing == null) return;

        Vector3 mouseWorldPosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        currentThing.transform.position = mouseWorldPosition;
        Time.timeScale = 0;
    }

public void OnCellClick(Cell cell)
    {
        if(currentThing == null) return;
        //currentThing.transform.position = cell.transform.position;
        bool isSuccess = cell.AddThingOS(currentThing);
        if (isSuccess)
        {
            Time.timeScale = 1;
            currentThing = null;
        }
    }

private void Update()
    {
        FollowCursor();
    }
}
