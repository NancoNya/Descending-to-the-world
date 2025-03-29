using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // 单例模式，方便在其他脚本中访问
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

        // 获取场景中所有带有AudioSource的物体
        bgmSources = FindObjectsOfType<AudioSource>();
    }

    // 播放BGM
    public void PlayBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Play();
        }
    }

    // 暂停BGM
    public void PauseBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Pause();
        }
    }

    // 停止BGM
    public void StopBGM()
    {
        foreach (AudioSource source in bgmSources)
        {
            source.Stop();
        }
    }

    // 设置BGM音量
    public void SetBGMVolume(float volume)
    {
        foreach (AudioSource source in bgmSources)
        {
            source.volume = volume;
        }
    }
}