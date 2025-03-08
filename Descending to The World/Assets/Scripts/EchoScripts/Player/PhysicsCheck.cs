using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("状态")]
    public bool isGround;

    [Header("检测通用数值")]
    public float checkRadius; // 物理检测范围大小

    [Header("地面监测")]
    public Vector2 bottomOffset; //检测偏移量
    public LayerMask groundLayer;

    private void Update()
    {
        Check();
    }


    public void Check()
    {
        isGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, checkRadius, groundLayer);
        //Physics2D.OverlapCapsule
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, checkRadius);
    }
}
