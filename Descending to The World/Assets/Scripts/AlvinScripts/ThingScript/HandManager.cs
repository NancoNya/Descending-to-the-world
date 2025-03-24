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

    // Ϊÿ�����������ṩ�����İ�ť�ӿ�
    public Button seismographButton;
    public Button seismometerButton;
    public Button kongMingLanternButton;
    //public Button sundialButton;
    public Button milkyWayButton;
    public Button compassButton;
    public Button magnetButton;
    public Button rocketButton;

    // �������Ͷ�Ӧ��Cellλ�ô洢�������������,˾�ϣ�
    public Dictionary<ThingOSType, Transform> propCellDict = new Dictionary<ThingOSType, Transform>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("HandManager ����ʵ���Ѿ����ڣ�");
            return;
        }
        instance = this;
        Debug.Log("HandManager ����ʵ����ʼ���ɹ�");
    }

    private void Start()
    {
        EventHandler.IdleEvent.AddListener(OnIdleEvent);

        // ��ÿ����ť��ӵ� buttonMap �ֵ���
        buttonMap[ThingOSType.Seismograph] = seismographButton;
        buttonMap[ThingOSType.Seismometer] = seismometerButton;
        buttonMap[ThingOSType.KongMingLantern] = kongMingLanternButton;
        //buttonMap[ThingOSType.Sundial] = sundialButton;
        buttonMap[ThingOSType.MilkyWay] = milkyWayButton;
        buttonMap[ThingOSType.Compass] = compassButton;
        buttonMap[ThingOSType.Magnet] = magnetButton;
        buttonMap[ThingOSType.Rocket] = rocketButton;

        // ����ֵ��Ƿ���ȷ��ʼ��
        foreach (var kvp in buttonMap)
        {
            if (kvp.Value != null)
            {
                Debug.Log($"�ɹ��� {kvp.Key} �밴ť����");
            }
            else
            {
                Debug.LogError($"δΪ {kvp.Key} ������Ч�İ�ť");
            }
        }
    }

    /// <summary>
    /// ����ʱ���������ѷ��õĿ����ơ������˾�ϡ���ʯ���ص����õ�
    /// </summary>
    private void OnIdleEvent()
    {
        // �����λ
        if (propCellDict.TryGetValue(ThingOSType.Rocket, out Transform rocketCell))
        {
            RocketIdle(rocketCell);
        }

        // �����Ƹ�λ
        if (propCellDict.TryGetValue(ThingOSType.KongMingLantern, out Transform kmdCell))
        {
            KongMingLanternIdle(kmdCell);
        }
        // ˾�ϸ�λ
        if (propCellDict.TryGetValue(ThingOSType.Compass, out Transform compassCell))
        {
            CompassIdle(compassCell);
        }
        // ��ʯ��λ
        if (propCellDict.TryGetValue(ThingOSType.Magnet, out Transform magnetCell))
        {
            MagnetIdle(magnetCell);
        }
    }

    private void MagnetIdle(Transform cellTransform)   ///// ��ʯ���ݸ�λ
    {
        foreach (Transform child in cellTransform)
        {
            ThingOnScene thing = child.GetComponent<ThingOnScene>();
            if (thing != null && thing.thingOSType == ThingOSType.Magnet && !thing.gameObject.activeSelf)
            {
                thing.transform.position = cellTransform.position;
                thing.gameObject.SetActive(true);
            }
        }
    }

    private void CompassIdle(Transform cellTransform)   ///// ˾�ϻ��ݸ�λ
    {
        foreach (Transform child in cellTransform)
        {
            ThingOnScene thing = child.GetComponent<ThingOnScene>(); 
            if (thing != null && thing.thingOSType == ThingOSType.Compass && !thing.gameObject.activeSelf)
            {
                thing.transform.position = cellTransform.position;
                thing.gameObject.SetActive(true);
            }
        }
    }

    private void RocketIdle(Transform cellTransform)   ///// ������ݸ�λ
    {
        foreach (Transform child in cellTransform)
        {
            ThingOnScene thing = child.GetComponent<ThingOnScene>();
            if (thing != null && thing.thingOSType == ThingOSType.Rocket && !thing.gameObject.activeSelf)
            {
                thing.gameObject.SetActive(true);
            }
        }
    }
    private void KongMingLanternIdle(Transform cellTransform)   ///// �����ƻ��ݸ�λ
    {
        foreach (Transform child in cellTransform)
        {
            ThingOnScene thing = child.GetComponent<ThingOnScene>();
            if (thing != null && thing.thingOSType == ThingOSType.KongMingLantern)
            {
                thing.transform.position = cellTransform.position;
                thing.GetComponent<Rigidbody2D>().velocity = Vector2.zero; // ֹͣ�˶�
            }
        }
    }

    public bool AddThingOS(ThingOSType thingOSType)
    {
        Debug.Log($"������� {thingOSType} ���͵ĵ���");
        if (currentThing != null)
        {
            Debug.Log("�������е��ߣ��޷����");
            return false;
        }
        ThingOnScene thingOSPrefab = GetThingOSPrefab(thingOSType);
        if (thingOSPrefab == null)
        {
            Debug.Log("δ�ҵ���Ӧ���͵ĵ���Ԥ����");
            return false;
        }
        currentThing = GameObject.Instantiate(thingOSPrefab);
        Debug.Log($"�ɹ���� {thingOSType} ���͵ĵ���");
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
            Debug.LogError("���߶����ѱ����٣��޷�������꣡");
            MarkThingAsDestroyed(currentThing.thingOSType);
            return;
        }
        if (Camera.main == null)
        {
            Debug.LogError("δ�ҵ��������");
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
            Debug.LogError($"��������������: {e.Message}");
        }
    }

    public void OnCellClick(Cell cell)
    {
        Debug.Log("���Խ����е��߷��õ���Ԫ��");
        if (currentThing == null)
        {
            Debug.Log("����û�е��ߣ��޷�����");
            return;
        }

        //////// ��¼�������Ͷ�Ӧ��Cellλ��
        if (currentThing.thingOSType == ThingOSType.KongMingLantern || currentThing.thingOSType == ThingOSType.Rocket || currentThing.thingOSType == ThingOSType.Compass || currentThing.thingOSType == ThingOSType.Magnet)
        {
            propCellDict[currentThing.thingOSType] = cell.transform;
        }

        bool isSuccess = cell.AddThingOS(currentThing);
        if (isSuccess)
        {
            Debug.Log("�ɹ������߷��õ���Ԫ��");
            Time.timeScale = 1;
            currentThing = null;
        }
        else
        {
            Debug.Log("�޷������߷��õ���Ԫ��");
        }
    }

    public void PickUpThingFromCell(Cell cell)
    {
        if (instance == null)
        {
            Debug.LogError("HandManager ����ʵ��δ��ʼ����");
            return;
        }
        Debug.Log("���Դӵ�Ԫ��ʰȡ����");
        if (currentThing != null)
        {
            Debug.Log($"�������е���: {currentThing.thingOSType}���޷�ʰȡ");
            return;
        }
        if (cell == null)
        {
            Debug.LogError("����ĵ�Ԫ�����Ϊ�գ�");
            return;
        }
        if (cell.currentThing == null)
        {
            Debug.Log("��Ԫ����û�е��ߣ��޷�ʰȡ");
            return;
        }
        Debug.Log($"׼��ʰȡ��Ԫ���еĵ���: {cell.currentThing.thingOSType}");
        Debug.Log("��ʼִ��ʰȡ����");
        currentThing = cell.currentThing;
        cell.currentThing = null;
        Time.timeScale = 0;
        Debug.Log("�ɹ��ӵ�Ԫ��ʰȡ����");
        if (currentThing != null)
        {
            Debug.Log($"��ǰ���е���: {currentThing.thingOSType}");
            if (currentThing.gameObject == null)
            {
                Debug.LogError("���߶����ѱ����٣�");
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
            Debug.LogError("currentThing δ��ȷ��ֵ��");
        }
    }

    public void MarkThingAsDestroyed(ThingOSType thingOSType)
    {
        if (buttonMap.ContainsKey(thingOSType))
        {
            Debug.Log($"���Լ��� {thingOSType} ��Ӧ�İ�ť");
            buttonMap[thingOSType].gameObject.SetActive(true);
            if (buttonMap[thingOSType].gameObject.activeSelf)
            {
                Debug.Log($" {thingOSType} ��Ӧ�İ�ť�ѳɹ�����");
            }
            else
            {
                Debug.LogError($" {thingOSType} ��Ӧ�İ�ť����ʧ��");
            }
        }
        else
        {
            Debug.LogError($"δ�ҵ� {thingOSType} ��Ӧ�İ�ť");
        }
    }

    public void HandleDoubleClickThing(ThingOSType thingOSType, GameObject thingObject)
    {
        Debug.Log($"���� {thingOSType} ���͵��ߵ��ٴε���¼�");
        MarkThingAsDestroyed(thingOSType);
        Destroy(thingObject);
    }

    private void Update()
    {
        FollowCursor();
    }
}