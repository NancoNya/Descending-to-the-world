using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    // 控制声音开关的方法
    public void ToggleSound(bool isOn)
    {
        audioSource.mute = !isOn;
    }

    void Update()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
