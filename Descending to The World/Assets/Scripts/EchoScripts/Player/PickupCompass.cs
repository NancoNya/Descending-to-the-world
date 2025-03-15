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
    /// 拾取司南
    /// </summary>
    public void PickUpCompass()
    {
        hasCompass = true;
        anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// 查找场景中有无磁石。若有，则向磁石方向移动；反之则播放拿着司南的动画，但保持原移动状态
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
    /// 已确认场景中放置磁石，人物向磁石方向移动
    /// </summary>
    private void MoveTowardsMagnet()
    {
        if (magnet != null)
        {
            // 获取当前人物和磁石的x轴位置
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
                // 磁石和人物x轴位置相同
                anim.SetBool("hasCompass", false);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}
