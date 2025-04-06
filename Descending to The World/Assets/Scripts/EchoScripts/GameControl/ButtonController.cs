using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    //[Header("����")]  // ������Ҫ�޸��߼�
    //public GenerateCompass generateCompass;
    [Header("��ʱ��")]
    public LevelTimer levelTimer;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// ���ʱ�Ӱ�ť
    /// </summary>
    private void OnButtonClick()
    {
        EventHandler.CallGameControlButtonClick();

        levelTimer.StartTimer();
    }
}