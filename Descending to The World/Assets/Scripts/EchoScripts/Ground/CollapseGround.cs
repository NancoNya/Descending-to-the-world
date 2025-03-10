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
    /// 人物走上坍塌地块,坍塌地块消失
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
    /// 点击时钟按钮，人物回溯，坍塌地块恢复
    /// </summary>
    private void OnResetGroundEvent()
    {
        //查找所有坍塌地块
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
