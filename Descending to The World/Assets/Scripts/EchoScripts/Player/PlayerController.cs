using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 initialPosition;
    public bool isMoving;

    // 道具
    public bool isHoldingCompass;
    private GameObject magnet;

    private Rigidbody2D rb;
    private Animator anim;
    private PhysicsCheck physicsCheck;
    public Vector3 faceDir;
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

    //private void Update()
    //{
    //    faceDir = new Vector3(-transform.localScale.x, 0, 0);

    //    if((physicsCheck.touchLeftWall && faceDir.x > 0) || (physicsCheck.touchRightWall && faceDir.x < 0))
    //    {
    //        transform.localScale = new Vector3(faceDir.x, 2, 1);
    //    }
    //}

    private void Update()
    {
        if (isHoldingCompass)
        {
            magnet = GameObject.FindWithTag("Magnet");
            if (magnet != null)
            {
                MoveTowardsMagnet();
            }
            else
            {
                anim.SetBool("IsHoldingCompass", true);
            }
        }
    }


    private void FixedUpdate()
    {
        // 移动, 在地面上, 没有拿着司南
        if(isMoving && physicsCheck.isGround && !isHoldingCompass)
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
    /// 拾取司南道具，向磁石移动
    /// </summary>
    private void MoveTowardsMagnet()
    {
        //磁石位置
        float targetX = magnet.transform.position.x;
        //人物当前
        float currentX = transform.position.x;
        //人物移动方向
        float direction = Mathf.Sign(targetX - currentX);

        if (Mathf.Abs(targetX - currentX) > 0.01f)
        {
            transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
            if (direction < 0 && transform.localScale.x > 0 || direction > 0 && transform.localScale.x < 0)
            {
                FlipDirection();
            }
        }
    }
}