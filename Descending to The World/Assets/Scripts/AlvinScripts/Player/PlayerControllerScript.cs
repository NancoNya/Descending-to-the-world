using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;//���Ͳ�ٶ�
    private float timer = 0f;
    private bool canAddSpeed = false;
    private GameObject Moon;
    private GameObject MilkyWay;
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
        // ���ĵ��ʱ�Ӵ������¼�
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
        // �ƶ�, �ڵ�����,δ�����ʯλ��
        if(isMoving && physicsCheckScript.isGround && !arriveMagnet)
        {
            //Debug.Log("fixupdate");
            WalkAnim();
            StartWalking();
        }

        // ���ڵ���������������
        else if (!physicsCheckScript.isGround)
        {
            //isMoving = true;
            FallDown();
        }

        // ʰȡ˾��
        if (hasCompass)
        {
            WalkAnim();
            MoveTowardsMagnet();
        }

        // �������
        if (canAddSpeed) 
        {
            rb.gravityScale = 0f;
            Debug.Log("������" + rb.gravityScale);
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
    /// �����ƶ��¼������ڵ��������ߣ����ڵ�����ֱ����
    /// </summary>
    void OnMovementEvent()
    {
        isMoving = true;
    }

    /// <summary>
    /// ���������¼�������ֹͣ�ƶ����ص���ʼλ��
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
    /// ���ʱ�ӣ�����ƶ��ٶ�
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
        //Debug.Log(moveSpeed);
        //Debug.Log(rb.velocity.x);
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
    /// Player�ƶ�ʱ���ʱ�ӣ��ص���ʼλ��
    /// </summary>
    public void BackToInitial()
    {
        rb.velocity = new Vector2(0f, 0f);
        Debug.Log("ˮƽ�ٶȣ�" + rb.velocity.x);
        //rb.velocity = Vector2.zero;
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
        if(collision.gameObject.CompareTag("Obstacle"))FlipDirection();
        if (collision.gameObject.CompareTag("Rocket"))
        {
            
            canAddSpeed = true;
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
        ///////anim.SetBool("hasCompass", true);
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
            //Debug.Log(hasCompass);
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
            float direction = (Mathf.Sign(targetX - currentX))*5;

            // ����sprite����
            if (direction != 0)
            {
                transform.localScale = new Vector3(direction, 5f, 1f);
            }
            // �����µ�λ��
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            // transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // �ж��Ƿ񵽴�Ŀ��λ��
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                //Debug.Log("arrive");
                // ����Ŀ��λ�ã�ֹͣ�ƶ������Ŵ�������
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
        if (other.gameObject.CompareTag("DeadZone"))   // ������䵽�߽�
        {
            OnIdleEvent();
            EventHandler.isMoving = !EventHandler.isMoving;
        }
        if (other.gameObject.CompareTag("CheckPoint"))  // ����С�ؿ��յ�
        {
            LevelManager.Instance.OnReachCheckpoint();
            gameObject.SetActive(false);
            other.enabled = false;
        }
    }

    /// <summary>
    /// ����ͨ�ص㣬ֹͣ�˶�
    /// </summary>
    public void SetHorizontalVelocityZero()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}