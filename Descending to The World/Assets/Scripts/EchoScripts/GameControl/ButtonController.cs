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
    /// µã»÷Ê±ÖÓ°´Å¥
    /// </summary>
    private void OnButtonClick()
    {
        EventHandler.CallGameControlButtonClick();
    }
}