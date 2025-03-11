using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseGround : MonoBehaviour
{
    //protected int reachCount = 0;
    private Collider2D groundCollider;

    private void Awake()
    {
        groundCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EventHandler.ResetGroundEvent.AddListener(OnResetGroundEvent);
    }

    //private void OnEnable()
    //{
    //    EventHandler.ResetGroundEvent.AddListener(OnResetGroundEvent);
    //}

    //private void OnDisable()
    //{
    //    EventHandler.ResetGroundEvent.RemoveListener(OnResetGroundEvent);
    //}


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
        //reachCount = 0;

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
