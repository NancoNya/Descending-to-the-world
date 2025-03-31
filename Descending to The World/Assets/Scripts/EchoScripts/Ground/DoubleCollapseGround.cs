using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCollapseGround : MonoBehaviour
{
    private Collider2D groundCollider;

    [Header("坍塌机制")]
    public int reachCount = 0;
    public bool firstReachCompleted;
    public float cooldownTime = 3f; // 冷却时间，单位为秒
    public float currentCooldown = 0f; // 当前剩余冷却时间

    private void Awake()
    {
        groundCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EventHandler.ResetEvent.AddListener(OnResetGroundEvent);
        firstReachCompleted = false;
    }

    private void OnDestroy()
    {
        EventHandler.ResetEvent.RemoveListener(OnResetGroundEvent);
    }

    /// <summary>
    /// 人物回溯，二次坍塌地块恢复，坍塌计数归零
    /// </summary>
    private void OnResetGroundEvent()
    {
        reachCount = 0;
        firstReachCompleted = false;
        currentCooldown = 0f; // 重置冷却时间

        //查找所有坍塌地块
        DoubleCollapseGround[] allDoubleCollapseGrounds = Resources.FindObjectsOfTypeAll<DoubleCollapseGround>();

        foreach (DoubleCollapseGround doubleCollapseGround in allDoubleCollapseGrounds)
        {
            if (doubleCollapseGround.CompareTag("CollapseGround"))
            {
                doubleCollapseGround.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        if (currentCooldown > 0f)  // 第一次踩上二次坍塌地块后，冷却时间开始计时，防止频繁触发
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public  void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!firstReachCompleted && currentCooldown < 0.01f)
            {
                reachCount++;
                firstReachCompleted = true;
                // Debug.Log(reachCount);
                currentCooldown = cooldownTime;
            }
            else if (reachCount == 1 && currentCooldown < 0.01f)
            {
                reachCount++;
                this.gameObject.SetActive(false);
            }
        }
    }
}
