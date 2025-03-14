using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GenerateCompass generateCompass;
    //public PickupCompass pickupCompass;
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
        
        GameObject compass = GameObject.FindWithTag("Compass");
        if (compass == null)
        {
            generateCompass.RespawnCompass();
        }
    }
}