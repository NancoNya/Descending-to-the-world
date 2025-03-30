using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public bool isMusicAllowedToPlay = true;
    private float lastVolume;
    private MusicController[] musicControllers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

        UpdateMusicControllers();
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 延迟 0.5 秒（可根据实际情况调整）
        await Task.Delay(500);
        lastVolume = GetCurrentVolume(); // 记录当前音量
        UpdateMusicControllers();
        SetBGMVolume(lastVolume); // 恢复之前的音量设置

        // 允许 MusicController 自动播放
        foreach (MusicController controller in musicControllers)
        {
            if (controller.audioSource != null)
            {
                controller.AllowAutoPlay();
            }
        }
    }

    private void UpdateMusicControllers()
    {
        musicControllers = FindObjectsOfType<MusicController>();
        Debug.Log($"当前场景获取到 {musicControllers.Length} 个 MusicController 组件");
    }

    // 播放 BGM
    public void PlayBGM()
    {
        isMusicAllowedToPlay = true;
        foreach (MusicController controller in musicControllers)
        {
            if (controller.audioSource != null)
            {
                controller.audioSource.Play();
            }
        }
    }

    // 暂停 BGM
    public void PauseBGM()
    {
        isMusicAllowedToPlay = false;
        foreach (MusicController controller in musicControllers)
        {
            if (controller.audioSource != null)
            {
                controller.audioSource.Pause();
            }
        }
    }

    // 停止 BGM
    public void StopBGM()
    {
        isMusicAllowedToPlay = false;
        foreach (MusicController controller in musicControllers)
        {
            if (controller.audioSource != null)
            {
                controller.audioSource.Stop();
            }
        }
    }

    // 设置 BGM 音量
    public void SetBGMVolume(float volume)
    {
        lastVolume = volume; // 更新记录的音量
        foreach (MusicController controller in musicControllers)
        {
            if (controller.audioSource != null)
            {
                controller.SetVolume(volume);
                Debug.Log($"已将 {controller.gameObject.name} 的音量设置为 {volume}，实际音量：{controller.audioSource.volume}");
            }
        }
    }

    // 获取当前音量
    public float GetCurrentVolume()
    {
        if (musicControllers.Length > 0 && musicControllers[0].audioSource != null)
        {
            return musicControllers[0].audioSource.volume;
        }
        return 0f;
    }
}