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

    [Header("������״̬")]
    public bool isHit;
    public bool canUse = false;  // �����ƶ�ʱ�����Ʋ����ƶ�
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
            // ����ԭ��ˮƽ�ٶ�
            float horizontalVelocity = playerRb.velocity.x;
            
            // ���ô�ֱ�ٶ�Ϊ�о��ٶ�
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
