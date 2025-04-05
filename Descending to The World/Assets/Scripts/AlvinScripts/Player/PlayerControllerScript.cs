using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;  // 火箭筒速度
    private float timer = 0f;


    [Header("初始设置")]
    public float moveSpeed;
    public Vector3 initialPosition;
    private float enlargeScale = 1.7f;   // sprite的scale放大倍数

    [Header("移动状态")]
    [SerializeField]private bool isMoving;
    [SerializeField]private Vector3 faceDir;
    public float currentDirection;  // 记录当前移动方向
    public bool canAddSpeed = false;
    //public bool isLantern;

    [Header("计时器")]
    [SerializeField]private float waitTime = 1f;
    [SerializeField]private float waitTimeCounter = 1f;
    public bool wait;
    public bool hasFlipped = false;

    [Header("道具持有状态")]
    public bool holdCompass = false;  // 是否拿着磁石
    [SerializeField]private bool arriveMagnet = false;
    [SerializeField]private bool moveToMagnet = false;  // 是否正在向磁石移动

    private GameObject magnet;
    private float positionThreshold = 0.01f; // 坐标差值

    private Rigidbody2D rb;
    private Animator anim;
    private PhysicsCheckScript physicsCheckScript;
    private bool isFacingRight = true;

    [Header("机关门")]
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    [Header("按钮")]
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    [Header("所处场景")]
    public int currentBigLevel;
    public int currentSmallLevel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheckScript = GetComponent<PhysicsCheckScript>();
    }

    private void OnEnable()
    {
        LevelManager.SceneSwitchedEvent += UpdatePlayerPosition;
    }

    private void OnDisable()
    {
        LevelManager.SceneSwitchedEvent += UpdatePlayerPosition;
    }

    void Start()
    {
        FallDown();
        //Moon = GameObject.Find("Moon");
        //MilkyWay = GameObject.Find("MilkyWayCollision");
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
        button1 = GameObject.Find("button1");
        button2 = GameObject.Find("button2");
        button3 = GameObject.Find("button3");
        door1 = GameObject.Find("door1");
        door2 = GameObject.Find("door2");
        door3 = GameObject.Find("door3");
        if (door1 == null && door2 == null && door3 == null)
            Debug.Log("该关卡中不存在机关门");
        
        // 订阅点击时钟触发的事件
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }

    private void UpdatePlayerPosition()
    {
        currentBigLevel = LevelManager.Instance.currentBigLevel;
        currentSmallLevel = LevelManager.Instance.currentSmallLevel;

        Debug.Log("大关卡: " + currentBigLevel);
        Debug.Log("小关卡: " + currentSmallLevel);

        int index = ((currentBigLevel - 1) * 2 + (currentSmallLevel - 1)) + 1;
        Debug.Log("关卡初始位置索引" + index);
        // 从 LevelInitialSO 中获取对应索引的人物初始位置
        if (LevelManager.Instance.levelInitialData != null && index < LevelManager.Instance.levelInitialData.playerPositions.Length)
        {
            if (this == null)
            {
                initialPosition = LevelManager.Instance.levelInitialData.playerPositions[index];
                // 将人物设置在对应场景的初始位置
                transform.position = initialPosition;
            }
        }
        else
            Debug.LogError("未能找到对应的初始位置");
    }

    private void Update()
    {
        TimeCounter();
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isMoving && (physicsCheckScript.isGround || physicsCheckScript.isLantern) && !moveToMagnet)   // 移动, 在地面上,不处于找磁石状态
        {
            anim.SetBool("isWalking", true);
            StartWalking();
        }
        if ((!physicsCheckScript.isGround && !canAddSpeed))    // 不在地面上,不在背着火箭，则自由落体
        {
            anim.SetBool("isWalking", false);
            FallDown();
        }
        
        if (holdCompass && moveToMagnet)    // 如果场景中存在磁石，则向磁石移动
        {
            MoveTowardsMagnet();  // PS：动画播放已在PickUpCompass()中执行
        }

        if (isMoving && physicsCheckScript.isGround && arriveMagnet)   // 到达磁石后，按当前移动方向继续移动
        {
            anim.SetBool("isWalking", true);
            StartWalking();
        }

        if (canAddSpeed)   // 火箭加速
        {
            Debug.Log("加速");
            anim.SetBool("hasRocket", true);
            timer += Time.fixedDeltaTime;
            rb.gravityScale = 0;
            rb.velocity = new Vector3(speed * currentDirection, 0, 0);
        }
        if (timer >= 0.8f)   // 加速时间结束 
        {
            anim.SetBool("hasRocket", false);
            anim.SetBool("isWalking", true);
            canAddSpeed = false;
            timer = 0;
            // rb.velocity = new Vector2(moveSpeed * currentDirection,0); 
            if(physicsCheckScript.isGround)
                StartWalking();
            if (!physicsCheckScript.isGround)
                FallDown();
            rb.gravityScale = 3f;
        }
    }

    /// <summary>
    /// 触发移动事件，若在地上则行走，不在地上则垂直下落
    /// </summary>
    void OnMovementEvent()
    {
        isMoving = true;
        currentDirection = 1f;
    }

    /// <summary>
    /// 触发回溯事件，人物停止移动，回到初始位置
    /// </summary>
    void OnIdleEvent()
    {
        currentDirection = 1f;
        rb.gravityScale = 3f;

        canAddSpeed = false;
        isMoving = false;
        holdCompass = false;
        moveToMagnet = false;
        arriveMagnet = false;
        // isLantern = false;
        physicsCheckScript.isLantern = false;

        anim.SetBool("hasCompass", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("hasRocket", false);

        BackToInitial();

        if (door1 != null)
            door1.SetActive(true);
        if (door2 != null)
            door2.SetActive(true);
        if (door3 != null)
            door3.SetActive(true);
    }

    /// <summary>
    /// 点击时钟，获得移动速度
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);
    }

    /// <summary>
    /// 垂直下落
    /// </summary>
    public void FallDown()
    {
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = 0;
        rb.velocity = currentVelocity;
    }

    /// <summary>
    /// Player移动时点击时钟，回到初始位置
    /// </summary>
    public void BackToInitial()
    {
        anim.SetBool("isWalking", false);
        rb.velocity = new Vector2(0f, 0f);
        transform.position = initialPosition;

        isFacingRight = true;
        moveSpeed = Mathf.Abs(moveSpeed);

        // 重置 timer 变量
        timer = 0f;

        //调整sprite方向
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x);
        transform.localScale = theScale;
    }

    /// <summary>
    /// 转换移动方向
    /// </summary>
    private void FlipDirection()
    {
        // 翻转运动方向
        currentDirection = -currentDirection;
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;
        // 翻转sprite
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// 拾取司南，查找场景中有无磁石
    /// </summary>
    public void PickUpCompass()
    {
        anim.SetBool("hasCompass", true);
        holdCompass = true;
        // 拾取司南后寻找磁石
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            moveToMagnet = true;
        }
    }

    /// <summary>
    /// 向磁石移动
    /// </summary>
    private void MoveTowardsMagnet()
    {
        if (magnet != null)
        {
            if (!physicsCheckScript.isGround)
            {
                FallDown();
                return;
            }
            float targetX = magnet.transform.position.x;
            float currentX = transform.position.x;

            // 计算移动方向 
            currentDirection = Mathf.Sign(targetX - currentX);
            // 设置sprite朝向
            if (currentDirection != 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x = currentDirection * enlargeScale;
                transform.localScale = theScale;
            }
            
            // 计算新的位置
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // 判断是否到达目标位置
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                // 动画切换：拿司南到走路
                anim.SetBool("hasCompass", false);
                anim.SetBool("isWalking", false);
                holdCompass = false;
                arriveMagnet = true;
                moveToMagnet = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == button1)
        {
            if (door1 != null)
            {
                door1.SetActive(false);
            }
        }
        else if (other.gameObject == button2)
        {
            if (door2 != null)
            {
                door2.SetActive(false);
            }
        }
        else if (other.gameObject == button3)
        {
            if (door3 != null)
            {
                door3.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))   // 人物碰到墙或地动仪
        {
            if (((physicsCheckScript.touchLeftWall && currentDirection < 0) || (physicsCheckScript.touchRightWall) && currentDirection > 0))
            {
                wait = true;
                hasFlipped = false;
            }
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.rb.gravityScale = 3;
        }
        if (collision.gameObject.CompareTag("Rocket"))   // 人物碰到火箭
        {
            if(!canAddSpeed)
            Debug.Log("前" + canAddSpeed);
            canAddSpeed = true;
            Debug.Log("后" + canAddSpeed);
        }
        if (collision.gameObject.CompareTag("DeadZone"))   // 人物掉落到边界
        {
            OnIdleEvent();
            EventHandler.ResetEvent.Invoke();   // 彩云，坍塌地块，场景道具复位
            EventHandler.CallTimerStopEvent();
            EventHandler.isMoving = !EventHandler.isMoving;
        }
        if (collision.gameObject.CompareTag("CheckPoint"))  // 到达小关卡终点
        {
            LevelManager.Instance.OnReachCheckpoint();
            collision.gameObject.SetActive(false);
        }
        if(physicsCheckScript.isLantern)
        {
            if (!physicsCheckScript.isGround)
            {
                FallDown();
                return;
            }
            rb.gravityScale = 0;
        }
        if(!physicsCheckScript.isLantern && !canAddSpeed)
        {
            if (!physicsCheckScript.isGround)
            {
                FallDown();
                return;
            }
            rb.gravityScale = 3f;
        }
    }

    public void TimeCounter()
    {

        if (wait)
        {
            if (!hasFlipped)
            {
                FlipDirection();
                hasFlipped = true;
            }
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
            }
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