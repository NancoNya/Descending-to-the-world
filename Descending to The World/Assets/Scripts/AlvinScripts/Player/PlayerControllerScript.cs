using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;//火箭筒速度
    private float timer = 0f;
    private bool canAddSpeed = false;
    private GameObject Moon;
    private GameObject MilkyWay;
    [Header("初始设置")]
    public float moveSpeed;
    public Vector3 initialPosition;

    [Header("状态")]
    public bool isMoving;
    public Vector3 faceDir;
    public bool hasCompass = false;
    public bool arriveMagnet = false;
    
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
        // 移动, 在地面上,未到达磁石位置
        if(isMoving && physicsCheckScript.isGround && !arriveMagnet)
        {
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

        // 拾取司南
        if (hasCompass)
        {
            WalkAnim();
            MoveTowardsMagnet();
        }

        // 火箭加速
        if (canAddSpeed) 
        {
            rb.gravityScale = 0f;
            Debug.Log("重力：" + rb.gravityScale);
            timer += Time.fixedDeltaTime; rb.velocity = new Vector3(speed, 0, 0); 
        }
        if (timer >= 0.8f)
        {
            rb.velocity = new Vector2(moveSpeed,0); 
            canAddSpeed = false;
            rb.gravityScale = 3f;
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
        canAddSpeed = false;
        isMoving = false;
        hasCompass = false;
        arriveMagnet = false;
        ////////////////////anim.SetBool("hasCompass", false);
        IdleAnim();
        BackToInitial();
        // rb.velocity = new Vector2(moveSpeed, 0f);
    }


    /// <summary>
    /// 点击时钟，获得移动速度
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
        //Debug.Log(moveSpeed);
        //Debug.Log(rb.velocity.x);
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
    }

    public void IdleAnim()
    {
        ///////////anim.SetBool("isWalking", false);
        //Debug.Log("idle");
    }


    /// <summary>
    /// Player移动时点击时钟，回到初始位置
    /// </summary>
    public void BackToInitial()
    {
        rb.velocity = new Vector2(0f, 0f);
        Debug.Log("水平速度：" + rb.velocity.x);
        //rb.velocity = Vector2.zero;
        transform.position = initialPosition;

        isFacingRight = true;
        moveSpeed = Mathf.Abs(moveSpeed);
        //Vector2 currentVelocity = rb.velocity;
        //currentVelocity.x = moveSpeed;
        //rb.velocity = currentVelocity;

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
        if(collision.gameObject.CompareTag("Obstacle"))FlipDirection();
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
        moveSpeed = -moveSpeed;
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;
        FlipSprite();
    }

    /// <summary>
    /// 人物sprite翻转
    /// </summary>
    private void FlipSprite()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// 拾取司南
    /// </summary>
    public void PickUpCompass()
    {
        // hasCompass = true;
        ///////anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// 拾取司南后寻找磁石。若找到磁石，向磁石方向移动；未找到则保持拿司南的动画
    /// </summary>
    private void FindMagnet()
    {
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            hasCompass = true;
            //Debug.Log(hasCompass);
        }
    }

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
            float direction = (Mathf.Sign(targetX - currentX))*5;

            // 设置sprite朝向
            if (direction != 0)
            {
                transform.localScale = new Vector3(direction, 5f, 1f);
            }
            // 计算新的位置
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            // transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // 判断是否到达目标位置
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                //Debug.Log("arrive");
                // 到达目标位置，停止移动并播放待机动画
                rb.velocity = Vector2.zero;
                ////////////////////////////anim.SetBool("hasCompass", false);
                ////////////////////////////anim.SetBool("isWalking", false);
                hasCompass = false;
                arriveMagnet = true;
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