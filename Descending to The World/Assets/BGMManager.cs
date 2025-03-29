using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Threading.Tasks;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;
    public bool isMusicAllowedToPlay = true;
    private float lastVolume; // �����������ڼ�¼��һ�ε�����

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
        // �ӳ� 0.5 �루�ɸ���ʵ�����������
        await Task.Delay(500);
        lastVolume = GetCurrentVolume(); // ��¼��ǰ����
        UpdateMusicControllers();
        SetBGMVolume(lastVolume); // �ָ�֮ǰ����������

        // ���� MusicController �Զ�����
        foreach (MusicController controller in musicControllers)
        {
            controller.AllowAutoPlay();
        }
    }

    private void UpdateMusicControllers()
    {
        musicControllers = FindObjectsOfType<MusicController>();
        Debug.Log($"��ǰ������ȡ�� {musicControllers.Length} �� MusicController ���");
    }

    // ���� BGM
    public void PlayBGM()
    {
        isMusicAllowedToPlay = true;
        foreach (MusicController controller in musicControllers)
        {
            controller.audioSource.Play();
        }
    }

    // ��ͣ BGM
    public void PauseBGM()
    {
        isMusicAllowedToPlay = false;
        foreach (MusicController controller in musicControllers)
        {
            controller.audioSource.Pause();
        }
    }

    // ֹͣ BGM
    public void StopBGM()
    {
        isMusicAllowedToPlay = false;
        foreach (MusicController controller in musicControllers)
        {
            controller.audioSource.Stop();
        }
    }

    // ���� BGM ����
    public void SetBGMVolume(float volume)
    {
        lastVolume = volume; // ���¼�¼������
        foreach (MusicController controller in musicControllers)
        {
            controller.SetVolume(volume);
            Debug.Log($"�ѽ� {controller.gameObject.name} ����������Ϊ {volume}��ʵ��������{controller.audioSource.volume}");
        }
    }

    // ��ȡ��ǰ����
    public float GetCurrentVolume()
    {
        if (musicControllers.Length > 0)
        {
            return musicControllers[0].audioSource.volume;
        }
        return 0f;
    }
}