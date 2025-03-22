using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("����")]  // ������Ҫ�޸��߼�
    public GenerateCompass generateCompass;
    [Header("��ʱ��")]
    public LevelTimer levelTimer;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        GameObject PropControllerObj = GameObject.FindWithTag("PropController");
        if (PropControllerObj != null)
        {
            generateCompass = PropControllerObj.GetComponent<GenerateCompass>();
        }
        else
        {
            Debug.LogError("δ�ҵ����� CompassSpawner �ű�����Ϸ����");
        }
    }

    /// <summary>
    /// ���ʱ�Ӱ�ť
    /// </summary>
    private void OnButtonClick()
    {
        EventHandler.CallGameControlButtonClick();

        levelTimer.StartTimer();

        GameObject compass = GameObject.FindWithTag("Compass");
        if (compass == null)
        {
            generateCompass.RespawnCompass();
        }
    }
}