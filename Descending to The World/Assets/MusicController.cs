using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private bool canAutoPlay = false; // 新增标志位

    private void Awake()
    {
        if (audioSource == null)
        {
            Debug.LogError("MusicController 中的 audioSource 未正确赋值！");
        }
    }

    public void AllowAutoPlay()
    {
        canAutoPlay = true;
    }

    // 设置音量的方法
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    void Update()
    {
        // 获取 BGMManager 实例
        BGMManager bgmManager = BGMManager.instance;
        if (bgmManager != null)
        {
            float currentVolume = bgmManager.GetCurrentVolume();
            Debug.Log($"当前音乐是否允许播放: {bgmManager.isMusicAllowedToPlay}，当前音量: {currentVolume}");

            // 只有当音乐被允许播放，音量大于 0，允许自动播放且音频未播放时才自动播放
            if (bgmManager.isMusicAllowedToPlay && canAutoPlay && !audioSource.isPlaying && audioSource.clip != null && currentVolume > 0)
            {
                Debug.Log("音乐被允许播放且音量大于 0，开始播放音乐");
                audioSource.Play();
            }
            else if (!bgmManager.isMusicAllowedToPlay)
            {
                Debug.Log("音乐不被允许播放，不执行播放操作");
            }
            else if (currentVolume == 0)
            {
                Debug.Log("音量为 0，不播放音乐");
            }
        }
    }
}