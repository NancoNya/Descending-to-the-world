using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongMingLantern : MonoBehaviour
{
    private float speed = 0.3f;
    private Rigidbody2D rb;
    private Rigidbody2D rb2;

    [Header("孔明灯状态")]
    public bool isHit;
    public bool canUse;  // 人物移动时孔明灯才能移动
    //public GameObject Moon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }

    private void OnDestroy()
    {
        EventHandler.MovementEvent.RemoveListener(OnMovementEvent);
        EventHandler.IdleEvent.RemoveListener(OnIdleEvent);
    }

    private void FixedUpdate()
    {
        //if (Moon)
        if(canUse)
        {
            rb.velocity = new Vector2(0, speed);
        }
        //else rb.velocity = new Vector2(0, 0);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 检测是否碰撞到tag为NormalFloor的对象
        if (collision.gameObject.CompareTag("Cloud"))
        {
            isHit = true;
            rb2 = collision.gameObject.GetComponent<Rigidbody2D>();
            //if (Moon)
            rb2.velocity = new Vector2(0, speed);
            //else rb2.velocity = new Vector2(0, 0);
            //Vector3 currentPosition = collision.gameObject.transform.position;

            //// 计算新的 Y 坐标，使其向上移动
            //float newYPosition = currentPosition.y + speed * Time.deltaTime;

            //// 更新物体的位置
            //transform.position = new Vector3(currentPosition.x, newYPosition, 0);
        }
    }

    private void Update()
    {
        //Moon = GameObject.Find("Moon");

    }

    private void OnIdleEvent()
    {
        canUse = false;
    }

    private void OnMovementEvent()
    {
        canUse = true;
    }
}
