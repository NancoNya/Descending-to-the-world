using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;  // ���Ͳ�ٶ�
    private float timer = 0f;
    private GameObject Moon;
    private GameObject MilkyWay;

    [Header("��ʼ����")]
    public float moveSpeed;
    public Vector3 initialPosition;
    private float enlargeScale = 5f;   // sprite��scale�Ŵ���

    [Header("�ƶ�״̬")]
    [SerializeField]private bool isMoving;
    [SerializeField]private Vector3 faceDir;
    [SerializeField]private float currentDirection;  // ��¼��ǰ�ƶ�����
    private bool canAddSpeed = false;

    [Header("���߳���״̬")]
    public bool holdCompass = false;  // �Ƿ����Ŵ�ʯ
    [SerializeField]private bool arriveMagnet = false;
    [SerializeField]private bool moveToMagnet = false;  // �Ƿ��������ʯ�ƶ�

    private GameObject magnet;
    private float positionThreshold = 0.01f; // �����ֵ

    private Rigidbody2D rb;
    private Animator anim;
    private PhysicsCheckScript physicsCheckScript;
    private bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
        currentDirection = Mathf.Sign(rb.velocity.x);
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
    }
    private void FixedUpdate()
    {
        
        if(isMoving && physicsCheckScript.isGround && !moveToMagnet)   // �ƶ�, �ڵ�����,δ�����ʯ״̬
        {
            //WalkAnim();
            anim.SetBool("isWalking", true);
            // currentDirection = 1f;
            StartWalking();
        }
        else if (!physicsCheckScript.isGround)    // ���ڵ���������������
        {
            anim.SetBool("isWalking", false);
            FallDown();
        }
        
        if (holdCompass && moveToMagnet)    // ��������д��ڴ�ʯ�������ʯ�ƶ�
        {
            MoveTowardsMagnet();  // PS��������������PickUpCompass()��ִ��
        }

        if (isMoving && physicsCheckScript.isGround && arriveMagnet)   // �����ʯ�󣬰���ǰ�ƶ���������ƶ�
        {
            //Debug.Log();
            //WalkAnim();
            anim.SetBool("isWalking", true);
            StartWalking();
        }

        if (canAddSpeed)   // �������
        {
            //RocketAnim();
            anim.SetBool("hasRocket", true);
            timer += Time.fixedDeltaTime; rb.velocity = new Vector3(speed, 0, 0);
        }
        if (timer >= 0.8f)   // ����ʱ����� 
        {
            //WalkAnim();
            anim.SetBool("hasRocket", false);
            anim.SetBool("isWalking", true);
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
        currentDirection = 1f;
        canAddSpeed = false;
        isMoving = false;
        holdCompass = false;
        moveToMagnet = false;
        arriveMagnet = false;
        anim.SetBool("hasCompass", false);
        BackToInitial();
    }

    /// <summary>
    /// ���ʱ�ӣ�����ƶ��ٶ�
    /// </summary>
    public void StartWalking()
    {
        rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);
    }

    /// <summary>
    /// ��ֱ����
    /// </summary>
    public void FallDown()
    {
        // anim.SetBool("isWalking", false);
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = 0;
        rb.velocity = currentVelocity;
        //IdleAnim();
    }

    //public void WalkAnim()
    //{
    //    anim.SetBool("isWalking", true);
    //    //anim.SetBool("hasCompass",false);
    //    //anim.SetBool("hasRocket", false);
    //}

    //public void IdleAnim()
    //{
    //    anim.SetBool("isWalking", false);
    //    anim.SetBool("hasCompass",false);
    //    anim.SetBool("hasRocket", false);
    //}

    //public void CompassAnim()
    //{
    //    anim.SetBool("hasCompass",true);
    //    anim.SetBool("isWalking", false);
    //    anim.SetBool("hasRocket", false);
    //}

    //public void RocketAnim()
    //{
    //    anim.SetBool("hasRocket", true);
    //    anim.SetBool("isWalking",false);
    //    anim.SetBool("hasCompass", false);
    //}

    /// <summary>
    /// Player�ƶ�ʱ���ʱ�ӣ��ص���ʼλ��
    /// </summary>
    public void BackToInitial()
    {
        anim.SetBool("isWalking", false);
        rb.velocity = new Vector2(0f, 0f);
        transform.position = initialPosition;

        isFacingRight = true;
        moveSpeed = Mathf.Abs(moveSpeed);
        //Vector2 currentVelocity = rb.velocity;
        //currentVelocity.x = moveSpeed;
        //rb.velocity = currentVelocity;

        // ���� timer ����
        timer = 0f;

        //����sprite����
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x);
        transform.localScale = theScale;
        //IdleAnim();
    }

    /// <summary>
    /// ��ײ��ǽ�ڻ�ض��Ƿ�ת
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
    /// ת���ƶ�����
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

        //// sprite��ת
        //Vector3 theScale = transform.localScale;
        //theScale.x = Mathf.Sign(moveSpeed) * enlargeScale;
        //transform.localScale = theScale;
        moveSpeed = -moveSpeed;
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// ʰȡ˾�ϣ����ҳ��������޴�ʯ
    /// </summary>
    public void PickUpCompass()
    {
        anim.SetBool("hasCompass", true);
        holdCompass = true;
        //CompassAnim();
        // ʰȡ˾�Ϻ�Ѱ�Ҵ�ʯ
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            moveToMagnet = true;
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
            currentDirection = Mathf.Sign(targetX - currentX);  // PS: ������������ͼƬ��С����

            // ����sprite����
            if (currentDirection != 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x = currentDirection * enlargeScale;
                transform.localScale = theScale;
            }
           
            // �����µ�λ��
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // �ж��Ƿ񵽴�Ŀ��λ��
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                //WalkAnim();
                // TODO �����л�����˾�ϵ���·
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