using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public static HandManager instance { get; private set; }

    public List<ThingOnScene> ThingOnSceneList;
    public ThingOnScene currentThing;
    public Dictionary<ThingOSType, Button> buttonMap = new Dictionary<ThingOSType, Button>();

    // 为每个道具类型提供公共的按钮接口
    public Button seismographButton;
    public Button kongMingLanternButton;
    public Button seismographButton1;
    public Button kongMingLanternButton1;
    public Button seismographButton2;
    public Button kongMingLanternButton2;
    //public Button sundialButton;
    public Button milkyWayButton;
    public Button compassButton;
    public Button magnetButton;
    public Button rocketButton;
    public Button rocketButton1;

    // 道具类型对应的Cell位置存储（火箭，孔明灯,司南）
    public Dictionary<ThingOSType, Transform> propCellDict = new Dictionary<ThingOSType, Transform>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("HandManager 单例实例已经存在！");
            return;
        }
        instance = this;
        Debug.Log("HandManager 单例实例初始化成功");
    }

    private void Start()
    {
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
        EventHandler.ResetEvent.AddListener(OnResetEvent);

        // 将每个按钮添加到 buttonMap 字典中
        buttonMap[ThingOSType.Seismograph] = seismographButton;
        buttonMap[ThingOSType.KongMingLantern] = kongMingLanternButton;
        //buttonMap[ThingOSType.Sundial] = sundialButton;
        buttonMap[ThingOSType.MilkyWay] = milkyWayButton;
        buttonMap[ThingOSType.Compass] = compassButton;
        buttonMap[ThingOSType.Magnet] = magnetButton;
        buttonMap[ThingOSType.Rocket] = rocketButton;
        buttonMap[ThingOSType.Seismograph1] = seismographButton1;
        buttonMap[ThingOSType.Seismograph2] = seismographButton2;
        buttonMap[ThingOSType.KongMingLantern1] = kongMingLanternButton1;
        buttonMap[ThingOSType.KongMingLantern2] = kongMingLanternButton2;
        buttonMap[ThingOSType.MilkyWay] = milkyWayButton;
        buttonMap[ThingOSType.Compass] = compassButton;
        buttonMap[ThingOSType.Magnet] = magnetButton;
        buttonMap[ThingOSType.Rocket1] = rocketButton1;

        // 检查字典是否正确初始化
        foreach (var kvp in buttonMap)
        {
            if (kvp.Value != null)
            {
                Debug.Log($"成功将 {kvp.Key} 与按钮关联");
            }
            else
            {
                Debug.LogError($"未为 {kvp.Key} 关联有效的按钮");
            }
        }
    }

    private void OnDestroy()
    {
        EventHandler.IdleEvent.RemoveListener(OnIdleEvent);
        EventHandler.ResetEvent.RemoveListener(OnResetEvent);
    }

    /// <summary>
    /// 人物掉出场景，道具复位
    /// </summary>
    private void OnResetEvent()
    {
        // 定义需要复位的道具类型数组
        ThingOSType[] thingOSTypes =
        {
            ThingOSType.Rocket,
            ThingOSType.KongMingLantern,
            ThingOSType.Compass,
            ThingOSType.Magnet
        };

        // 遍历道具类型，进行复位操作
        foreach (ThingOSType type in thingOSTypes)
        {
            if (propCellDict.TryGetValue(type, out Transform cellTransform))
            {
                ResetThingOnScene(type, cellTransform);
            }
        }
    }

    /// <summary>
    /// 回溯时，场景中已放置的孔明灯、火箭、司南。磁石都回到放置点
    /// </summary>
    private void OnIdleEvent()
    {
        // 定义需要复位的道具类型数组
        ThingOSType[] thingOSTypes = 
        {
            ThingOSType.Rocket,
            ThingOSType.KongMingLantern,
            ThingOSType.Compass,
            ThingOSType.Magnet
        };

        // 遍历道具类型，进行复位操作
        foreach (ThingOSType type in thingOSTypes)
        {
            if (propCellDict.TryGetValue(type, out Transform cellTransform))
            {
                ResetThingOnScene(type, cellTransform);
            }
        }
    }

    /// <summary>
    /// 通用道具复位方法
    /// </summary>
    /// <param name="thingOSType">需要复位的道具类型</param>
    /// <param name="cellTransform">道具所在单元格的Transform</param>
    private void ResetThingOnScene(ThingOSType thingOSType, Transform cellTransform)
    {
        foreach (Transform child in cellTransform)
        {
            ThingOnScene thing = child.GetComponent<ThingOnScene>();
            if (thing != null && thing.thingOSType == thingOSType)
            {
                switch (thingOSType)
                {
                    case ThingOSType.Rocket:
                        if (!thing.gameObject.activeSelf)
                        {
                            thing.gameObject.SetActive(true);
                        }
                        break;
                    case ThingOSType.KongMingLantern:
                        thing.transform.position = cellTransform.position;
                        thing.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 停止运动
                        break;
                    case ThingOSType.Compass:
                        if (!thing.gameObject.activeSelf)
                        {
                            thing.transform.position = cellTransform.position;
                            thing.gameObject.SetActive(true);
                        }
                        break;
                    case ThingOSType.Magnet:
                        if (!thing.gameObject.activeSelf)
                        {
                            thing.transform.position = cellTransform.position;
                            thing.gameObject.SetActive(true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public bool AddThingOS(ThingOSType thingOSType)
    {
        Debug.Log($"尝试添加 {thingOSType} 类型的道具");
        if (currentThing != null)
        {
            Debug.Log("手中已有道具，无法添加");
            return false;
        }
        ThingOnScene thingOSPrefab = GetThingOSPrefab(thingOSType);
        if (thingOSPrefab == null)
        {
            Debug.Log("未找到对应类型的道具预制体");
            return false;
        }
        currentThing = GameObject.Instantiate(thingOSPrefab);
        Debug.Log($"成功添加 {thingOSType} 类型的道具");
        if (buttonMap.ContainsKey(thingOSType))
        {
            buttonMap[thingOSType].gameObject.SetActive(false);
        }
        return true;
    }

    private ThingOnScene GetThingOSPrefab(ThingOSType thingOSType)
    {
        foreach (ThingOnScene thingOnScene in ThingOnSceneList)
            if (thingOnScene.thingOSType == thingOSType) return thingOnScene;
        return null;
    }

    void FollowCursor()
    {
        if (currentThing == null)
        {
            return;
        }
        if (currentThing.gameObject == null)
        {
            Debug.LogError("道具对象已被销毁，无法跟随鼠标！");
            MarkThingAsDestroyed(currentThing.thingOSType);
            return;
        }
        if (Camera.main == null)
        {
            Debug.LogError("未找到主相机！");
            return;
        }
        try
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;
            currentThing.transform.position = mouseWorldPosition;
            Time.timeScale = 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"跟随鼠标操作出错: {e.Message}");
        }
    }

    public void OnCellClick(Cell cell)
    {
        Debug.Log("尝试将手中道具放置到单元格");
        if (currentThing == null)
        {
            Debug.Log("手中没有道具，无法放置");
            return;
        }

        //////// 记录道具类型对应的Cell位置
        if (currentThing.thingOSType == ThingOSType.KongMingLantern || currentThing.thingOSType == ThingOSType.Rocket || currentThing.thingOSType == ThingOSType.Compass || currentThing.thingOSType == ThingOSType.Magnet)
        {
            propCellDict[currentThing.thingOSType] = cell.transform;
        }

        bool isSuccess = cell.AddThingOS(currentThing);
        if (isSuccess)
        {
            Debug.Log("成功将道具放置到单元格");
            Time.timeScale = 1;
            currentThing = null;
        }
        else
        {
            Debug.Log("无法将道具放置到单元格");
        }
    }

    public void PickUpThingFromCell(Cell cell)
    {
        if (instance == null)
        {
            Debug.LogError("HandManager 单例实例未初始化！");
            return;
        }
        Debug.Log("尝试从单元格拾取道具");
        if (currentThing != null)
        {
            Debug.Log($"手中已有道具: {currentThing.thingOSType}，无法拾取");
            return;
        }
        if (cell == null)
        {
            Debug.LogError("传入的单元格对象为空！");
            return;
        }
        if (cell.currentThing == null)
        {
            Debug.Log("单元格中没有道具，无法拾取");
            return;
        }
        Debug.Log($"准备拾取单元格中的道具: {cell.currentThing.thingOSType}");
        Debug.Log("开始执行拾取操作");
        currentThing = cell.currentThing;
        cell.currentThing = null;
        Time.timeScale = 0;
        Debug.Log("成功从单元格拾取道具");
        if (currentThing != null)
        {
            Debug.Log($"当前手中道具: {currentThing.thingOSType}");
            if (currentThing.gameObject == null)
            {
                Debug.LogError("道具对象已被销毁！");
                MarkThingAsDestroyed(currentThing.thingOSType);
            }
            else
            {
                if (buttonMap.ContainsKey(currentThing.thingOSType))
                {
                    buttonMap[currentThing.thingOSType].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogError("currentThing 未正确赋值！");
        }
    }

    public void MarkThingAsDestroyed(ThingOSType thingOSType)
    {
        if (buttonMap.ContainsKey(thingOSType))
        {
            Debug.Log($"尝试激活 {thingOSType} 对应的按钮");
            buttonMap[thingOSType].gameObject.SetActive(true);
            if (buttonMap[thingOSType].gameObject.activeSelf)
            {
                Debug.Log($" {thingOSType} 对应的按钮已成功激活");
            }
            else
            {
                Debug.LogError($" {thingOSType} 对应的按钮激活失败");
            }
        }
        else
        {
            Debug.LogError($"未找到 {thingOSType} 对应的按钮");
        }
    }

    public void HandleDoubleClickThing(ThingOSType thingOSType, GameObject thingObject)
    {
        Debug.Log($"处理 {thingOSType} 类型道具的再次点击事件");
        MarkThingAsDestroyed(thingOSType);
        Destroy(thingObject);
    }

    private void Update()
    {
        FollowCursor();
    }
}