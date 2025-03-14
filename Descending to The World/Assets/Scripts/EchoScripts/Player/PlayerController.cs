using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("��ʼ����")]
    public float moveSpeed;
    public Vector3 initialPosition;

    [Header("״̬")]
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
        // ���ĵ��ʱ�Ӵ������¼�
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
        // �ƶ�, �ڵ�����
        if(isMoving && physicsCheck.isGround)
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

    /// <summary>
    /// ���������¼�������ֹͣ�ƶ����ص���ʼλ��
    /// </summary>
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
    /// ʰȡ˾��
    /// </summary>
    public void PickUpCompass()
    {
        hasCompass = true;
        anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// ʰȡ˾�Ϻ�Ѱ�Ҵ�ʯ�����ҵ���ʯ�����ʯ�����ƶ�
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
            //δ�ҵ���ʯ���ƶ�״̬���䣬���Բ�������˾�ϵĶ���
            anim.SetBool("hasCompass", true);
        }
    }

    /// <summary>
    /// ���ʯ�ƶ�
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