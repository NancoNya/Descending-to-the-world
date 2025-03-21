using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager instance { get; private set; }

    public List<ThingOnScene> ThingOnSceneList;
    public List<ThingPickUp> ThingPickUpList;
    public ThingOnScene currentThing;
    public ThingPickUp currentPickUp;
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

    public bool AddThingOS(ThingPickUpType thingPickUpType)
    {
        if (currentPickUp != null) return false;
        ThingPickUp thingPickUpPrefab = GetThingOSPrefab(thingPickUpType);
        if (thingPickUpPrefab == null) { print("Null"); return false; }
        currentPickUp = GameObject.Instantiate(thingPickUpPrefab);
        return true;
    }

    //public bool MoveThingOS(ThingOSType thingOSType)
    //{
    //    if (currentThing != null) return false;
    //    ThingOnScene thingOSPrefab = GetThingOSPrefab(thingOSType);
    //    if (thingOSPrefab == null) { print("Null"); return false; }
    //    currentThing = GameObject.Instantiate(thingOSPrefab);
    //    return true;
    //}


    private ThingOnScene GetThingOSPrefab(ThingOSType thingOSType)
    {
        foreach(ThingOnScene thingOnScene in ThingOnSceneList)
        if(thingOnScene.thingOSType == thingOSType)return thingOnScene;
        return null;
    }

    private ThingPickUp GetThingOSPrefab(ThingPickUpType thingPickUpType)
    {
        foreach (ThingPickUp thingPickUp in ThingPickUpList)
            if (thingPickUp.thingPickUpType == thingPickUpType) return thingPickUp;
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
    void FollowCursor2()
    {
        if (currentPickUp == null) return;
        Vector3 mouseWorldPosition2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition2.z = 0;
        currentPickUp.transform.position = mouseWorldPosition2;
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

public void OnCellClick2(Cell cell)
    {
        if (currentPickUp == null) return;
        //currentThing.transform.position = cell.transform.position;
        bool isSuccess = cell.AddThingOS(currentPickUp);
        if (isSuccess)
        {
            Time.timeScale = 1;
            currentThing = null;
        }
    }

    //public void OnCellClickAgain(Cell cell)
    //{
    //    if (currentThing != null) return;
    //    //currentThing.transform.position = cell.transform.position;
    //    bool isSuccessAgain = cell.MoveThingOS(currentThing);
    //    if (isSuccessAgain)
    //    {
    //        Time.timeScale = 0;
    //        foreach (ThingOnScene thingOnScene in ThingOnSceneList)
    //            if (thingOnScene.name == cell.Name) { currentThing = thingOnScene; Debug.Log(thingOnScene.name); }
    //    }
    //}

    private void Update()
    {
        FollowCursor();
        FollowCursor2();
    }
}
