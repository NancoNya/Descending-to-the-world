using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCollapseGround : MonoBehaviour
{
    private Collider2D groundCollider;
    public int reachCount = 0;

    private void Awake()
    {
        groundCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EventHandler.ResetGroundEvent.AddListener(OnResetGroundEvent);
    }

    //private void OnDisable()
    //{
    //    EventHandler.ResetGroundEvent.RemoveListener(OnResetGroundEvent);
    //}

    /// <summary>
    /// 人物回溯，二次坍塌地块恢复，坍塌计数归零
    /// </summary>
    private void OnResetGroundEvent()
    {
        reachCount = 0;
        //if (groundCollider != null)
        //{
        //    groundCollider.enabled = true;
        //}
        if(! this.gameObject.activeSelf)
            gameObject.SetActive(true);
    }


    public  void OnCollisionEnter2D(Collision2D collision)
    {
        //base.OnTriggerEnter2D(collision);
        if (collision.gameObject.CompareTag("Player"))
        {
            reachCount++;
            Debug.Log(reachCount);
            if (reachCount == 1)
            {
                StartCoroutine(DisableCollider());
            }
            if (reachCount == 2)
            {
                //groundCollider.enabled = false;
                this.gameObject.SetActive(false);
                Debug.Log("remove doubleCollapseGround");
            }      
        }
    }

    System.Collections.IEnumerator DisableCollider()
    {
        groundCollider.enabled = false;
        yield return new WaitForSeconds(3f);
        // 等待三秒后启动
        groundCollider.enabled = true;
    }


}
