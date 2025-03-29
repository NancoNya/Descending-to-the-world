using UnityEngine;
using UnityEngine.UI;

public class MusicControlButton : MonoBehaviour
{
    private Button button;
    private bool isMusicOn = true;
    private float normalVolume = 1f; // 正常音量值，可根据需要调整

    private void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ToggleMusicVolume);
        }
    }

    private void ToggleMusicVolume()
    {
        if (isMusicOn)
        {
            // 关闭音乐声音
            BGMManager.instance.SetBGMVolume(0f);
        }
        else
        {
            // 恢复正常音量
            BGMManager.instance.SetBGMVolume(normalVolume);
        }
        // 切换状态
        isMusicOn = !isMusicOn;
    }
}