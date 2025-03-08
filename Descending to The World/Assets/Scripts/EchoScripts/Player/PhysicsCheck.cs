using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("״̬")]
    public bool isGround;

    [Header("���ͨ����ֵ")]
    public float checkRadius; // �����ⷶΧ��С

    [Header("������")]
    public Vector2 bottomOffset; //���ƫ����
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
