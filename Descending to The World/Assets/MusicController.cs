using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource audioSource;
    private bool canAutoPlay = false; // ������־λ

    private void Awake()
    {
        if (audioSource == null)
        {
            Debug.LogError("MusicController �е� audioSource δ��ȷ��ֵ��");
        }
    }

    public void AllowAutoPlay()
    {
        canAutoPlay = true;
    }

    // ���������ķ���
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    void Update()
    {
        // ��ȡ BGMManager ʵ��
        BGMManager bgmManager = BGMManager.instance;
        if (bgmManager != null)
        {
            float currentVolume = bgmManager.GetCurrentVolume();
            Debug.Log($"��ǰ�����Ƿ�������: {bgmManager.isMusicAllowedToPlay}����ǰ����: {currentVolume}");

            // ֻ�е����ֱ������ţ��������� 0�������Զ���������Ƶδ����ʱ���Զ�����
            if (bgmManager.isMusicAllowedToPlay && canAutoPlay && !audioSource.isPlaying && audioSource.clip != null && currentVolume > 0)
            {
                Debug.Log("���ֱ����������������� 0����ʼ��������");
                audioSource.Play();
            }
            else if (!bgmManager.isMusicAllowedToPlay)
            {
                Debug.Log("���ֲ��������ţ���ִ�в��Ų���");
            }
            else if (currentVolume == 0)
            {
                Debug.Log("����Ϊ 0������������");
            }
        }
    }
}