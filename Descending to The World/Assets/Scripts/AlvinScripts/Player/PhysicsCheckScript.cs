using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheckScript : MonoBehaviour
{
    [Header("״̬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;

    [Header("���ͨ����ֵ")]
    public float checkRadius; // �����ⷶΧ��С

    [Header("������")]
    public Vector2 bottomOffset; //���ƫ����
    public LayerMask groundLayer;

    [Header("ǽ�ں͵ض��Ǽ��")]
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
