using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class KongMingLantern : MonoBehaviour
{
    private float liftSpeed = 1f;
    private Rigidbody2D rb;
    private Rigidbody2D playerRb;
    public PlayerControllerScript playerController;
    public PhysicsCheckScript physicsCheck;

    [Header("孔明灯状态")]
    public bool isHit;
    public bool canUse = false;  // 人物移动时孔明灯才能移动
    //public GameObject Moon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
        EventHandler.ResetEvent.AddListener(OnResetEvent);
    }

    private void OnDestroy()
    {
        EventHandler.MovementEvent.RemoveListener(OnMovementEvent);
        EventHandler.IdleEvent.RemoveListener(OnIdleEvent);
        EventHandler.ResetEvent.RemoveListener(OnResetEvent);

    }

    private void FixedUpdate()
    {
        //if (Moon)
        if(canUse)
        {
            rb.velocity = new Vector2(0, liftSpeed);
        }

        if (isHit && playerRb != null && canUse)
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
            playerController = collision.gameObject.GetComponent<PlayerControllerScript>();
            physicsCheck = collision.gameObject.GetComponent<PhysicsCheckScript>();
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
        if (physicsCheck != null && !physicsCheck.isLantern && playerController != null)
        {
            playerController.FallDown();
        }
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

    private void OnResetEvent()
    {
        canUse = false;
        isHit = false;
    }
}
