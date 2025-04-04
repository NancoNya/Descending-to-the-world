using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongMingLantern : MonoBehaviour
{
    private float liftSpeed = 5f;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;

    [Header("孔明灯状态")]
    public bool isHit;
    public bool canUse = false;  // 人物移动时孔明灯才能移动
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
            rb.velocity = new Vector2(0, liftSpeed);
        }

        if (isHit && playerRb != null)
        {
            // 保持原有水平速度
            float horizontalVelocity = playerRb.velocity.x;
            
            // 设置垂直速度为托举速度
            playerRb.velocity = new Vector2(horizontalVelocity, liftSpeed);
        }
        //else rb.velocity = new Vector2(0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.gravityScale = 0;
            isHit = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isHit = false;
            playerRb.gravityScale = 3f;
            playerRb = null;
            
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // 检测是否碰撞到tag为NormalFloor的对象
    //    if (collision.gameObject.CompareTag("Cloud"))
    //    {
    //        isHit = true;
    //        rb2 = collision.gameObject.GetComponent<Rigidbody2D>();
    //        //if (Moon)
    //        rb2.velocity = new Vector2(0, speed);
    //        //else rb2.velocity = new Vector2(0, 0);
    //        //Vector3 currentPosition = collision.gameObject.transform.position;

    //        //// 计算新的 Y 坐标，使其向上移动
    //        //float newYPosition = currentPosition.y + speed * Time.deltaTime;

    //        //// 更新物体的位置
    //        //transform.position = new Vector3(currentPosition.x, newYPosition, 0);
    //    }
    //}

    private void Update()
    {
        //Moon = GameObject.Find("Moon");

    }

    private void OnIdleEvent()
    {
        canUse = false;
        isHit = false;
    }

    private void OnMovementEvent()
    {
        canUse = true;
    }
}
