using UnityEngine;
using UnityEngine.UI;

public class MusicControlButton : MonoBehaviour
{
    private Button button;
    private bool isMusicOn = true;
    private float normalVolume = 1f;
    public Sprite UIOpen;
    public Sprite UIClose;
    private Image buttonImage;
    private void Start()
    {
        buttonImage = GetComponent<Image>();
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
            
            BGMManager.instance.SetBGMVolume(0f);
            buttonImage.sprite = UIClose;
        }
        else
        {
            BGMManager.instance.SetBGMVolume(normalVolume);
            buttonImage.sprite = UIOpen;
        }
        isMusicOn = !isMusicOn;
    }
}