using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
    private int EscTime = 0;
    public GameObject PauseMenu;

    //Scene MenuScene = SceneManager.GetSceneByName("PropColumn");
    //注意修改该loadScene
    public void Next()
    {
        //Scene scene = SceneManager.GetSceneByName("GroundBigScene");
        SceneManager.LoadScene("GroundBigScene");
    }
    public void Back()
    {
        //Scene scene = SceneManager.GetSceneByName("SkyBigScene");
        SceneManager.LoadScene("SkyBigScene");
    }
    public void GameStart1()
    {
        Scene scene = SceneManager.GetSceneByName("1.1");
        SceneManager.LoadScene("1.1");
        //Scene MenuScene = SceneManager.GetSceneByName("PropColumn");
        //Scene scene = SceneManager.GetSceneByName("PropColumn");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 1;
        LevelManager.Instance.currentSmallLevel = 1;
    }
    public void GameStart2()
    {
        Scene scene = SceneManager.GetSceneByName("2.1");
        SceneManager.LoadScene("2.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 2;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart3()
    {
        Scene scene = SceneManager.GetSceneByName("3.1");
        SceneManager.LoadScene("3.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 3;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart4()
    {
        Scene scene = SceneManager.GetSceneByName("4.1");
        SceneManager.LoadScene("4.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 4;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart5()
    {
        Scene scene = SceneManager.GetSceneByName("5.1");
        SceneManager.LoadScene("5.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 5;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart6()
    {
        Scene scene = SceneManager.GetSceneByName("6.1");
        SceneManager.LoadScene("6.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 6;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart7()
    {
        Scene scene = SceneManager.GetSceneByName("7.1");
        SceneManager.LoadScene("7.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 7;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart8()
    {
        Scene scene = SceneManager.GetSceneByName("8.1");
        SceneManager.LoadScene("8.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 8;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart9()
    {
        Scene scene = SceneManager.GetSceneByName("9.1");
        SceneManager.LoadScene("9.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 9;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart10()
    {
        Scene scene = SceneManager.GetSceneByName("10.1");
        SceneManager.LoadScene("10.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 10;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart11()
    {
        Scene scene = SceneManager.GetSceneByName("11.1");
        SceneManager.LoadScene("11.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 11;
        LevelManager.Instance.currentSmallLevel = 1;
    }

    public void GameStart12()
    {
        Scene scene = SceneManager.GetSceneByName("12.1");
        SceneManager.LoadScene("12.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
        SceneManager.LoadScene("Persistent", LoadSceneMode.Additive);
        LevelManager.Instance.currentBigLevel = 12;
        LevelManager.Instance.currentSmallLevel = 1;
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
        PauseMenu.SetActive(false); }
        private void Update()
    {
        EscTime %= 2;
        if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 0)
        {
            EscTime++;
            GamePause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 1)
        {
            EscTime++;
            GameContinue();
        }
    }

}

    //}

    //public void GameBack()
    //{
    //    SceneManager.LoadScene(0);
    //}
    //private void Start()
    //{
    //    //PauseMenu.SetActive(false);
    //}
    //private void Update()
    //{
    //    EscTime %= 2;
    //    if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 0)
    //    {
    //        EscTime++;
    //        GamePause();
    //    }else if(Input.GetKeyDown(KeyCode.Escape)&&EscTime == 1)
    //    {
    //        EscTime++;
    //        GameContinue();
    //    }
    //}
