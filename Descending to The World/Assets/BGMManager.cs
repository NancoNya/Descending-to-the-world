using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // ����ģʽ�������������ű��з���
    public static BGMManager instance;

    private AudioSource[] bgmSources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ȡ���������д���AudioSource������
        bgmSources = FindObjectsOfType<AudioSource>();
    }

    // ����BGM
    public void PlayBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Play();
        }
    }

    // ��ͣBGM
    public void PauseBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Pause();
        }
    }

    // ֹͣBGM
    public void StopBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Stop();
        }
    }

    // ����BGM����
    public void SetBGMVolume(float volume)
    {
        foreach (AudioSource source in bgmSources)
        {
            source.volume = volume;
        }
    }
}