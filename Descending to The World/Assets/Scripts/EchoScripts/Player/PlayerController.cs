using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 initialPosition;
    public bool isMoving;

    // ����
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
        // ���ĵ��ʱ�Ӵ������¼�
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
        // �ƶ�, �ڵ�����, û������˾��
        if(isMoving && physicsCheck.isGround && !isHoldingCompass)
        {
            WalkAnim();
            StartWalking();
        }

        // ���ڵ���������������
        else if (!physicsCheck.isGround)
        {
            //isMoving = true;
            FallDown();
        }
    }

    /// <summary>
    /// �����ƶ��¼������ڵ��������ߣ����ڵ�����ֱ����
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
        //    Debug.Log("���ڵ���");
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
    /// ���ʱ�ӣ�����ƶ��ٶ�
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    /// <summary>
    /// ��ֱ����
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
    /// Player�ƶ�ʱ���ʱ�ӣ��ص���ʼλ��
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

        //����sprite����
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x);
        transform.localScale = theScale;

    }

    /// <summary>
    /// ��ײ��ǽ�ڻ�ض��Ƿ�ת
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
    /// ת���ƶ�����
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
    /// ����sprite��ת
    /// </summary>
    private void FlipSprite()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// ʰȡ˾�ϵ��ߣ����ʯ�ƶ�
    /// </summary>
    private void MoveTowardsMagnet()
    {
        //��ʯλ��
        float targetX = magnet.transform.position.x;
        //���ﵱǰ
        float currentX = transform.position.x;
        //�����ƶ�����
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