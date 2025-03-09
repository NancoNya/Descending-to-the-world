using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 initialPosition;
    public bool isMoving;

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

    private void FixedUpdate()
    {
        // 移动并且在地面上
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
        Debug.Log("falldown");
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


    private void FlipDirection()
    {
        moveSpeed = -moveSpeed;
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;
        FlipSprite();
    }

    private void FlipSprite()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}