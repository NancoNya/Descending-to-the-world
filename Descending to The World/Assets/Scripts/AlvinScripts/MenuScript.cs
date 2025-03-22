using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private int EscTime = 0;
    public GameObject PauseMenu;
    public void GameStart()
    {
        SceneManager.LoadScene(1);
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
        SceneManager.LoadScene(0);
    }

    public void GameBackToSky()
    {
        SceneManager.LoadScene(1);
    }
    private void Start()
    {
        //PauseMenu.SetActive(false);
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
