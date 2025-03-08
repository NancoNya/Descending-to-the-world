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
    /// µã»÷Ê±ÖÓ°´Å¥
    /// </summary>
    void OnButtonClick()
    {
        EventHandler.OnGameControlButtonClick();
    }
}