using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HelpScript : MonoBehaviour
{
    private int EscTime = 0;
    public GameObject Next1;
    public GameObject Next2;
    public GameObject Next3;
    public GameObject Next4;
    public Canvas HelpCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        EscTime %= 2;
        if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 0 && HelpCanvas.gameObject.activeSelf)
        {
            EscTime++;
            HelpStart();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && EscTime == 1 && HelpCanvas.gameObject.activeSelf)
        {
            EscTime++;
            HelpExit();
        }
    }

    #region Next Help
    public void NextHelp1()
    {
        Next1.SetActive(true);
        Next2.SetActive(false);
        Next3.SetActive(false);
        Next4.SetActive(false);

    }

    public void NextHelp2()
    {
        Next2.SetActive(true);
        Next1.SetActive(false);
        Next3.SetActive(false);
        Next4.SetActive(false);

    }

    public void NextHelp3()
    {
        Next3.SetActive(true);
        Next2.SetActive(false);
        Next1.SetActive(false);
        Next4.SetActive(false);

    }

    public void NextHelp4()
    {
        Next4.SetActive(true);
        Next2.SetActive(false);
        Next1.SetActive(false);
        Next3.SetActive(false);

    }
    #endregion

    public void HelpStart()
    {
        HelpCanvas.gameObject.SetActive(true);
    }

    public void HelpExit()
    {
        HelpCanvas.gameObject.SetActive(false);
    }

    
}
