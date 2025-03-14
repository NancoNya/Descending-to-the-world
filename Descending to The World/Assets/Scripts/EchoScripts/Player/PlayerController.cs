using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("初始设置")]
    public float moveSpeed;
    public Vector3 initialPosition;

    [Header("状态")]
    public bool isMoving;
    public Vector3 faceDir;
    public bool hasCompass;
    
    private GameObject magnet;

    private Rigidbody2D rb;
    private Animator anim;
    private PhysicsCheck physicsCheck;
    private bool isFacingRight = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    void Start()
    {
        // 订阅点击时钟触发的事件
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }


    private void Update()
    {
        if (hasCompass)
        {
            magnet = GameObject.FindWithTag("Magnet");
            if (magnet != null)
            {
                MoveTowardsMagnet();
            }
            //else
            //{
            //    anim.SetBool("hasCompass", true);
            //}
        }
    }


    private void FixedUpdate()
    {
        // 移动, 在地面上
        if(isMoving && physicsCheck.isGround)
        {
            WalkAnim();
            StartWalking();
        }

        // 不在地面上则自由落体
        else if (!physicsCheck.isGround)
        {
            //isMoving = true;
            FallDown();
        }
    }

    /// <summary>
    /// 触发移动事件，若在地上则行走，不在地上则垂直下落
    /// </summary>
    void OnMovementEvent()
    {
        isMoving = true;
        //if(physicsCheck.isGround)
        //{
        //    WalkAnim();
        //}
        //else if(!physicsCheck.isGround)
        //{
        //    Debug.Log("不在地上");
        //    FallDown();
        //    IdleAnim();
        //}
    }

    /// <summary>
    /// 触发回溯事件，人物停止移动，回到初始位置
    /// </summary>
    void OnIdleEvent()
    {
        isMoving = false;
        IdleAnim();
        BackToInitial();
    }


    /// <summary>
    /// 点击时钟，获得移动速度
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
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
        anim.SetBool("isWalking", false);
    }

    public void WalkAnim()
    {
        anim.SetBool("isWalking", true);
    }

    public void IdleAnim()
    {
        anim.SetBool("isWalking", false);
    }

    /// <summary>
    /// Player移动时点击时钟，回到初始位置
    /// </summary>
    public void BackToInitial()
    {
        rb.velocity = Vector2.zero;
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
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            FlipDirection();
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
        hasCompass = true;
        anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// 拾取司南后寻找磁石。若找到磁石，向磁石方向移动
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
            //未找到磁石，移动状态不变，但仍播放拿着司南的动画
            anim.SetBool("hasCompass", true);
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
            else
            {
                anim.SetBool("hasCompass", false);
                //anim.SetBool("isIdle", true);
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }
}