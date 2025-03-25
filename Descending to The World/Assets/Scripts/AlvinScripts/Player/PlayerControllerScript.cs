using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;  // 火箭筒速度
    private float timer = 0f;
    private bool canAddSpeed = false;
    private GameObject Moon;
    private GameObject MilkyWay;

    [Header("初始设置")]
    public float moveSpeed;
    public Vector3 initialPosition;
    private int enlargeScale = 5;   // sprite的scale放大倍数

    [Header("状态")]
    public bool isMoving;
    public Vector3 faceDir;
    public bool hasCompass = false;  // 是否拿着磁石
    public bool arriveMagnet = false;
    // public bool afterArriveMagnet = false;
    public bool moveToMagnet = false;  // 是否正在向磁石移动
    [SerializeField]private float currentDirection; // 记录向磁石移动时的移动方向

    private GameObject magnet;
    private float positionThreshold = 0.01f; // 坐标差值

    private Rigidbody2D rb;
    //private Animator anim;
    private PhysicsCheckScript physicsCheckScript;
    private bool isFacingRight = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        physicsCheckScript = GetComponent<PhysicsCheckScript>();
    }

    void Start()
    {
        //Moon = GameObject.Find("Moon");
        //MilkyWay = GameObject.Find("MilkyWayCollision");
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
        // 订阅点击时钟触发的事件
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }

    private void Update()
    {
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
    }
    private void FixedUpdate()
    {
        // 移动, 在地面上,未到达磁石状态
        if(isMoving && physicsCheckScript.isGround && !arriveMagnet)
        {
            currentDirection = 1f;  // PS: 根据最终人物图片大小调整
            //Debug.Log("fixupdate");
            WalkAnim();
            StartWalking();
        }

        // 不在地面上则自由落体
        else if (!physicsCheckScript.isGround)
        {
            //isMoving = true;
            FallDown();
        }

        // 如果场景中存在磁石，则向磁石移动
        if (hasCompass && moveToMagnet) 
        {
            MoveTowardsMagnet();  // PS：动画播放已在PickUpCompass()中执行
        }

        // 到达磁石后，按当前移动方向继续移动
        if (isMoving && physicsCheckScript.isGround && arriveMagnet)
        {
            WalkAnim();
            // rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);
            StartWalking();
        }

        // 火箭加速
        if (canAddSpeed) 
        {
            Debug.Log("重力：" + rb.gravityScale);
            timer += Time.fixedDeltaTime; rb.velocity = new Vector3(speed, 0, 0);
            // TODO: 播放背火箭动画
        }
        if (timer >= 0.8f)
        {
            rb.velocity = new Vector2(moveSpeed,0); 
            canAddSpeed = false;
            rb.gravityScale = 3f;
            // TODO: 播放走路动画
        }
    }

    /// <summary>
    /// 触发移动事件，若在地上则行走，不在地上则垂直下落
    /// </summary>
    void OnMovementEvent()
    {
        isMoving = true;
    }

    /// <summary>
    /// 触发回溯事件，人物停止移动，回到初始位置
    /// </summary>
    void OnIdleEvent()
    {
        currentDirection = 1f;  // PS: 根据最终人物图片大小调整
        canAddSpeed = false;
        isMoving = false;
        hasCompass = false;
        // arriveMagnet = false;
        //afterArriveMagnet = false;
        moveToMagnet = false;
        ////////////////////anim.SetBool("hasCompass", false);
        IdleAnim();
        BackToInitial();
    }


    /// <summary>
    /// 点击时钟，获得移动速度
    /// </summary>
    public void StartWalking()
    {
            rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);  // PS: 根据最终人物图片大小调整
    }

    /// <summary>
    /// 垂直下落
    /// </summary>
    public void FallDown()
    {
        Vector2 currentVelocity = rb.velocity;
        //rb.velocity = new Vector2(0,rb.velocity.y);
        currentVelocity.x = 0;
        rb.velocity = currentVelocity;
        //anim.SetBool("isWalking", false);
    }

    public void WalkAnim()
    {
        ///////////anim.SetBool("isWalking", true);
        ///anim.SetBool("hasCompass",false);
    }

    public void IdleAnim()
    {
        ///////////anim.SetBool("isWalking", false);
        //////anim.SetBool("hasCompass",false);
        //Debug.Log("idle");
    }

    public void CompassAnim()
    {
        // anim.SetBool("hasCompass",true);
        // anim.SetBool("isWalking", false);
    }
    /// <summary>
    /// Player移动时点击时钟，回到初始位置
    /// </summary>
    public void BackToInitial()
    {
        rb.velocity = new Vector2(0f, 0f);
        transform.position = initialPosition;

        isFacingRight = true;
        moveSpeed = Mathf.Abs(moveSpeed);
        //Vector2 currentVelocity = rb.velocity;
        //currentVelocity.x = moveSpeed;
        //rb.velocity = currentVelocity;

        // 重置 timer 变量
        timer = 0f;

        //调整sprite方向
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x);
        transform.localScale = theScale;
    }

    /// <summary>
    /// 碰撞到墙壁或地动仪翻转
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if((physicsCheckScript.touchLeftWall || physicsCheckScript.touchRightWall))
                FlipDirection();
        }
        if (collision.gameObject.CompareTag("Rocket"))
        {
            canAddSpeed = true;
        }
    }

    /// <summary>
    /// 转换移动方向
    /// </summary>
    private void FlipDirection()
    {
        //Vector2 currentVelocity = rb.velocity;

        //moveSpeed = -moveSpeed;
        //currentDirection = Mathf.Sign(moveSpeed);

        //currentVelocity.x = moveSpeed;
        //rb.velocity = currentVelocity;
        //isFacingRight = !isFacingRight;
        ////FlipSprite();

        //// sprite翻转
        //Vector3 theScale = transform.localScale;
        //theScale.x = Mathf.Sign(moveSpeed) * enlargeScale;
        //transform.localScale = theScale;
        Debug.Log(moveSpeed);
        moveSpeed = -moveSpeed;
        Debug.Log(moveSpeed);
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// 人物sprite翻转
    /// </summary>
    //private void FlipSprite()
    //{
    //    Vector3 theScale = transform.localScale;
    //    theScale.x *= -1;
    //    transform.localScale = theScale;
    //}

    /// <summary>
    /// 拾取司南，查找场景中有无磁石
    /// </summary>
    public void PickUpCompass()
    {
        hasCompass = true;
        ///////CompassAnim();
        // FindMagnet();
        // 拾取后寻找磁石
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            //hasCompass = true;
            moveToMagnet = true;
        }
    }

    /// <summary>
    /// 拾取司南后寻找磁石。
    /// </summary>
    //private void FindMagnet()
    //{
    //    magnet = GameObject.FindWithTag("Magnet");
    //    if (magnet != null)
    //    {
    //        //hasCompass = true;
    //        moveToMagnet = true;
    //    }
    //}

    /// <summary>
    /// 向磁石移动
    /// </summary>
    private void MoveTowardsMagnet()
    {
        if (magnet != null)
        {
            float targetX = magnet.transform.position.x;
            float currentX = transform.position.x;

            // 计算移动方向
            //float direction = (Mathf.Sign(targetX - currentX))*5;

            // 计算移动方向 
            currentDirection = Mathf.Sign(targetX - currentX);  // PS: 根据最终人物图片大小调整

            // 设置sprite朝向
            if (currentDirection != 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x = currentDirection * enlargeScale;
                transform.localScale = theScale;
                //transform.localScale = new Vector3(currentDirection * enlargeScale, enlargeScale, 1f);  // PS: 根据最终人物图片大小调整
            }
            //// 设置sprite朝向
            //if (currentDirection != 0)
            //{
            //    transform.localScale = new Vector3(direction, 5f, 1f);
            //}
            // 计算新的位置
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // 判断是否到达目标位置
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                //Debug.Log("arrive");
                // 到达目标位置，停止移动并播放待机动画
                // rb.velocity = Vector2.zero;
                ////////////////////////////anim.SetBool("hasCompass", false);
                ////////////////////////////anim.SetBool("isWalking", false);
                hasCompass = false;
                // TODO 动画切换：拿司南到走路
                arriveMagnet = true;
                moveToMagnet = false;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("DeadZone"))   // 人物掉落到边界
        {
            OnIdleEvent();
            EventHandler.isMoving = !EventHandler.isMoving;
        }
        if (other.gameObject.CompareTag("CheckPoint"))  // 到达小关卡终点
        {
            LevelManager.Instance.OnReachCheckpoint();
            gameObject.SetActive(false);
            other.enabled = false;
        }
    }

    /// <summary>
    /// 到达通关点，停止运动
    /// </summary>
    public void SetHorizontalVelocityZero()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}