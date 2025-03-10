using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseGround : MonoBehaviour
{
    private void Start()
    {
        EventHandler.ResetGroundEvent.AddListener(OnResetGroundEvent);
    }

    /// <summary>
    /// ��������̮���ؿ�,̮���ؿ���ʧ
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// ���ʱ�Ӱ�ť��������ݣ�̮���ؿ�ָ�
    /// </summary>
    private void OnResetGroundEvent()
    {
        //��������̮���ؿ�
        CollapseGround[] allCollapseGrounds = Resources.FindObjectsOfTypeAll<CollapseGround>();

        foreach (CollapseGround collapseGround in allCollapseGrounds)
        {
            if (collapseGround.CompareTag("CollapseGround"))
            {
                collapseGround.gameObject.SetActive(true);
            }
        }

        //foreach (var collapseGround in FindObjectsOfType<CollapseGround>())
        //{
        //    collapseGround.gameObject.SetActive(true);
        //}
    }
}
