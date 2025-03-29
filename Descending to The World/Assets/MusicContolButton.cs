using UnityEngine;
using UnityEngine.UI;

public class MusicControlButton : MonoBehaviour
{
    private Button button;
    private bool isMusicOn = true;
    private float normalVolume = 1f; // ��������ֵ���ɸ�����Ҫ����

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
            // �ر���������
            BGMManager.instance.SetBGMVolume(0f);
        }
        else
        {
            // �ָ���������
            BGMManager.instance.SetBGMVolume(normalVolume);
        }
        // �л�״̬
        isMusicOn = !isMusicOn;
    }
}