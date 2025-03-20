using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
    private int EscTime = 0;
    public GameObject PauseMenu;

    //×¢ÒâÐÞ¸Ä¸ÃloadScene
    public void GameStart1()
    {
        SceneManager.LoadScene(2);
    }
    public void GameStart2()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart3()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart5()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart6()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart7()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart8()
    {
        SceneManager.LoadScene(2);
    }

    public void GameStart4()
    {
        SceneManager.LoadScene(2);
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
