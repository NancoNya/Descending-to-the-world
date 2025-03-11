using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
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
    }
}