using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("��ʼ����")]
    public float moveSpeed;
    public Vector3 initialPosition;

    [Header("״̬")]
    public bool isMoving;
    public Vector3 faceDir;
    public bool hasCompass = false;
    public bool arriveMagnet = false;
    
    private GameObject magnet;
    private float positionThreshold = 0.01f; // �����ֵ

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

    private void FixedUpdate()
    {
        // �ƶ�, �ڵ�����,δ�����ʯλ��
        if(isMoving && physicsCheck.isGround && !arriveMagnet)
        {
            Debug.Log("fixupdate");
            WalkAnim();
            StartWalking();
        }

        // ���ڵ���������������
        else if (!physicsCheck.isGround)
        {
            //isMoving = true;
            FallDown();
        }

        if (hasCompass)
        {
            WalkAnim();
            MoveTowardsMagnet();
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
        hasCompass = false;
        arriveMagnet = false;
        anim.SetBool("hasCompass", false);
        IdleAnim();
        BackToInitial();
    }


    /// <summary>
    /// ���ʱ�ӣ�����ƶ��ٶ�
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
        Debug.Log(moveSpeed);
        Debug.Log(rb.velocity.x);
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
        Debug.Log("idle");
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
        // hasCompass = true;
        anim.SetBool("hasCompass", true);
        FindMagnet();
    }

    /// <summary>
    /// ʰȡ˾�Ϻ�Ѱ�Ҵ�ʯ�����ҵ���ʯ�����ʯ�����ƶ���δ�ҵ��򱣳���˾�ϵĶ���
    /// </summary>
    private void FindMagnet()
    {
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            hasCompass = true;
            Debug.Log(hasCompass);
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

            // �����ƶ�����
            float direction = Mathf.Sign(targetX - currentX);

            // ����sprite����
            if (direction != 0)
            {
                transform.localScale = new Vector3(direction, 1f, 1f);
            }
            // �����µ�λ��
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            // transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // �ж��Ƿ񵽴�Ŀ��λ��
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                Debug.Log("arrive");
                // ����Ŀ��λ�ã�ֹͣ�ƶ������Ŵ�������
                rb.velocity = Vector2.zero;
                anim.SetBool("hasCompass", false);
                anim.SetBool("isWalking", false);
                hasCompass = false;
                arriveMagnet = true;
            }
            //if (currentX < targetX)
            //{
            //    transform.localScale = new Vector3(1f, 1f, 1f);
            //    transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            //}
            //else if (currentX > targetX)
            //{
            //    transform.localScale = new Vector3(-1f, 1f, 1f);
            //    transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
            //}
            //else
            //{
            //    anim.SetBool("hasCompass", false);
            //    hasCompass = false;
            //    GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //}
        }
    }
}