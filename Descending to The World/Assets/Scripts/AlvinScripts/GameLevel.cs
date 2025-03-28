using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLevel : MonoBehaviour
{
    private int EscTime = 0;
    public GameObject PauseMenu;
    Scene MenuScene = SceneManager.GetSceneByName("PropColumn");
    //×¢ÒâÐÞ¸Ä¸ÃloadScene
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

    }
    public void GameStart2()
    {
        Scene scene = SceneManager.GetSceneByName("2.1");
        SceneManager.LoadScene("2.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart3()
    {
        Scene scene = SceneManager.GetSceneByName("3.1");
        SceneManager.LoadScene("3.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart4()
    {
        Scene scene = SceneManager.GetSceneByName("4.1");
        SceneManager.LoadScene("4.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart5()
    {
        Scene scene = SceneManager.GetSceneByName("5.1");
        SceneManager.LoadScene("5.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart6()
    {
        Scene scene = SceneManager.GetSceneByName("6.1");
        SceneManager.LoadScene("6.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart7()
    {
        Scene scene = SceneManager.GetSceneByName("7.1");
        SceneManager.LoadScene("7.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart8()
    {
        Scene scene = SceneManager.GetSceneByName("8.1");
        SceneManager.LoadScene("8.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart9()
    {
        Scene scene = SceneManager.GetSceneByName("9.1");
        SceneManager.LoadScene("9.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart10()
    {
        Scene scene = SceneManager.GetSceneByName("10.1");
        SceneManager.LoadScene("10.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart11()
    {
        Scene scene = SceneManager.GetSceneByName("11.1");
        SceneManager.LoadScene("11.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameStart12()
    {
        Scene scene = SceneManager.GetSceneByName("12.1");
        SceneManager.LoadScene("12.1");
        SceneManager.LoadScene("PropColumn", LoadSceneMode.Additive);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    //public void GamePause()
    //{
    //    PauseMenu.SetActive(true);
    //    Time.timeScale = 0;
        
    //}

    //public void GameContinue()
    //{
    //    Time.timeScale = 1;
    //    PauseMenu.SetActive(false);
        
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
}
