using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseGround : MonoBehaviour
{
    private Collider2D groundCollider;

    private void Awake()
    {
        groundCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EventHandler.ResetEvent.AddListener(OnResetGroundEvent);
    }

    private void OnDestroy()
    {
        EventHandler.ResetEvent.RemoveListener(OnResetGroundEvent);
    }
    
    /// <summary>
    /// ��������̮���ؿ�,̮���ؿ���ʧ
    /// </summary>
    /// <param name="collision"></param>
    public  void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
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
        Debug.Log(allCollapseGrounds);
        foreach (CollapseGround collapseGround in allCollapseGrounds)
        {
            if (collapseGround.CompareTag("CollapseGround"))
            {
                collapseGround.gameObject.SetActive(true);
            }
        }
    }
}
