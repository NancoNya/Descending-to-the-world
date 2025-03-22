using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KongMingLantern : MonoBehaviour
{
    private float speed = 0.3f;
    private Rigidbody2D rb;
    private Rigidbody2D rb2;
    public bool isHit;
    public GameObject Moon;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (Moon)
            rb.velocity = new Vector2(0, speed);
        else rb.velocity = new Vector2(0, 0);

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 检测是否碰撞到tag为NormalFloor的对象
        if (collision.gameObject.CompareTag("NormalFloor"))
        {
            isHit = true;
            rb2 = collision.gameObject.GetComponent<Rigidbody2D>();
            if (Moon)rb2.velocity = new Vector2(0, speed);
            else rb2.velocity = new Vector2(0, 0);
            //Vector3 currentPosition = collision.gameObject.transform.position;

            //// 计算新的 Y 坐标，使其向上移动
            //float newYPosition = currentPosition.y + speed * Time.deltaTime;

            //// 更新物体的位置
            //transform.position = new Vector3(currentPosition.x, newYPosition, 0);
        }
    }
    private void Update()
    {
        Moon = GameObject.Find("Moon");

    }
}
