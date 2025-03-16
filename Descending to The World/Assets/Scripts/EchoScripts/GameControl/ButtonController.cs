using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public GenerateCompass generateCompass;
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
            Debug.LogError("未找到挂载 CompassSpawner 脚本的游戏对象");
        }
    }

    /// <summary>
    /// 点击时钟按钮
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