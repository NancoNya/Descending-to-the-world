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
        lastVolume = GetCurrentVolume();
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 延迟一段时间确保 MusicController 初始化完成
        await Task.Delay(500);
        UpdateMusicControllers();

        // 恢复之前的音量设置
        if (musicControllers.Length > 0)
        {
            SetBGMVolume(lastVolume);
        }

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
        return lastVolume;
    }
}