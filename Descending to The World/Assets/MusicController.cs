using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;

    // �����������صķ���
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
