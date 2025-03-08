using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// ���ʱ�Ӱ�ť
    /// </summary>
    void OnButtonClick()
    {
        EventHandler.OnGameControlButtonClick();
    }
}