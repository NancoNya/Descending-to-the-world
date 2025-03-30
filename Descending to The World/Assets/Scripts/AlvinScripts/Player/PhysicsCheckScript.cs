using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheckScript : MonoBehaviour
{
    [Header("状态")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    [Header("检测通用数值")]
    public float checkRadius; // 物理检测范围大小

    [Header("地面监测")]
    public Vector2 bottomOffset; //检测偏移量
    public LayerMask groundLayer;

    [Header("墙壁和地动仪检测")]
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask obstacleLayer;


    private void Update()
    {
        Check();
    }


    public void Check()
    {
        // isGround = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, groundLayer);
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRadius, obstacleLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRadius, obstacleLayer);
    }

    private void OnDrawGizmosSelected()
    {
        // Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CollapseGround"))
            isGround = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CollapseGround"))
            isGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("CollapseGround"))
            isGround = false;
    }
}
