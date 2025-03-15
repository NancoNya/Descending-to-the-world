using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCompass : MonoBehaviour
{
    private float moveSpeed;
    private Animator anim;
    public bool hasCompass = false;
    private GameObject magnet;
    private PlayerController playerController;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        moveSpeed = playerController.moveSpeed;
    }


    private void Update()
    {
        if (hasCompass)
        {
            MoveTowardsMagnet();
        }
    }


    /// <summary>
    /// ʰȡ˾��
    /// </summary>
    public void PickUpCompass()
    {
        hasCompass = true;
        anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// ���ҳ��������޴�ʯ�����У������ʯ�����ƶ�����֮�򲥷�����˾�ϵĶ�����������ԭ�ƶ�״̬
    /// </summary>
    private void FindMagnet()
    {
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            MoveTowardsMagnet();
        }
        else
        {
            anim.SetBool("hasCompass", true);
        }
    }

    /// <summary>
    /// ��ȷ�ϳ����з��ô�ʯ���������ʯ�����ƶ�
    /// </summary>
    private void MoveTowardsMagnet()
    {
        if (magnet != null)
        {
            // ��ȡ��ǰ����ʹ�ʯ��x��λ��
            float targetX = magnet.transform.position.x;
            float currentX = transform.position.x;
            if (Mathf.Abs(currentX - targetX) > 0.01f)
            {
                if (currentX < targetX)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                }
                else if (currentX > targetX)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                // ��ʯ������x��λ����ͬ
                anim.SetBool("hasCompass", false);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
