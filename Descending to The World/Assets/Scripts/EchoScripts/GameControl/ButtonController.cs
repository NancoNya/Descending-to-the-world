using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    //[Header("道具")]  // 后续需要修改逻辑
    //public GenerateCompass generateCompass;
    [Header("计时器")]
    public LevelTimer levelTimer;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// 点击时钟按钮
    /// </summary>
    private void OnButtonClick()
    {
        EventHandler.CallGameControlButtonClick();

        levelTimer.StartTimer();
    }
}