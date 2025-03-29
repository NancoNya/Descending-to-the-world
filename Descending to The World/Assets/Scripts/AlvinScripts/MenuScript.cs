using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject Setting;
    private int EscTime = 0;
    public GameObject PauseMenu;

    public void GameStart()
    {
        Scene scene = SceneManager.GetSceneByName("SkyBigScene");
        SceneManager.LoadScene("SkyBigScene");
    }

    public void GameSetting()
    {
        Setting.SetActive(true);
        Debug.Log("is click");
    }

    public void GameSettingBack()
    {
        Setting.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void GamePause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;

    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);

    }

    public void GameBack()
    {
        SceneManager.LoadScene("SkyBigScene");
    }

    public void GameExitMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //public void GameBackToSky()
    //{
    //    SceneManager.LoadScene("SkyBigScene");
    //}

    private void Start()
    {
        Setting.SetActive(false);
    }
    private void Update()
    {
        EscTime %= 2;
        if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 0)
        {
            EscTime++;
            GamePause();
        }else if(Input.GetKeyDown(KeyCode.Escape)&&EscTime == 1)
        {
            EscTime++;
            GameContinue();
        }
    }
}
