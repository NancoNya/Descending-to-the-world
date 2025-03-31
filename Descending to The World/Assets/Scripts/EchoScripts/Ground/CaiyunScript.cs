using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaiyunScript : MonoBehaviour
{
    private Vector3 initialPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        EventHandler.ResetEvent.AddListener(OnResetCaiYunEvent);
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDestroy()
    {
        EventHandler.ResetEvent.RemoveListener(OnResetCaiYunEvent);
    }

    /// <summary>
    /// 回溯，彩云回到初始位置
    /// </summary>
    private void OnResetCaiYunEvent()
    {
        transform.position = initialPosition;
        rb.velocity = new Vector2(0, 0);
    }
}
