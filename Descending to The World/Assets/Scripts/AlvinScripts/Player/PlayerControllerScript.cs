using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;  // ���Ͳ�ٶ�
    private float timer = 0f;


    [Header("��ʼ����")]
    public float moveSpeed;
    public Vector3 initialPosition;
    private float enlargeScale = 1.7f;   // sprite��scale�Ŵ���

    [Header("�ƶ�״̬")]
    [SerializeField]private bool isMoving;
    [SerializeField]private Vector3 faceDir;
    public float currentDirection;  // ��¼��ǰ�ƶ�����
    public bool canAddSpeed = false;
    //public bool isLantern;

    [Header("��ʱ��")]
    [SerializeField]private float waitTime = 1f;
    [SerializeField]private float waitTimeCounter = 1f;
    public bool wait;
    public bool hasFlipped = false;

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

    [Header("������")]
    public GameObject door1;
    public GameObject door2;
    public GameObject door3;

    [Header("��ť")]
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    [Header("��������")]
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
            Debug.Log("�ùؿ��в����ڻ�����");
        
        // ���ĵ��ʱ�Ӵ������¼�
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }

    private void UpdatePlayerPosition()
    {
        currentBigLevel = LevelManager.Instance.currentBigLevel;
        currentSmallLevel = LevelManager.Instance.currentSmallLevel;

        Debug.Log("��ؿ�: " + currentBigLevel);
        Debug.Log("С�ؿ�: " + currentSmallLevel);

        int index = ((currentBigLevel - 1) * 2 + (currentSmallLevel - 1)) + 1;
        Debug.Log("�ؿ���ʼλ������" + index);
        // �� LevelInitialSO �л�ȡ��Ӧ�����������ʼλ��
        if (LevelManager.Instance.levelInitialData != null && index < LevelManager.Instance.levelInitialData.playerPositions.Length)
        {
            if (this == null)
            {
                initialPosition = LevelManager.Instance.levelInitialData.playerPositions[index];
                // �����������ڶ�Ӧ�����ĳ�ʼλ��
                transform.position = initialPosition;
            }
        }
        else
            Debug.LogError("δ���ҵ���Ӧ�ĳ�ʼλ��");
    }

    private void Update()
    {
        TimeCounter();
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isMoving && (physicsCheckScript.isGround || physicsCheckScript.isLantern) && !moveToMagnet)   // �ƶ�, �ڵ�����,�������Ҵ�ʯ״̬
        {
            anim.SetBool("isWalking", true);
            StartWalking();
        }
        if ((!physicsCheckScript.isGround && !canAddSpeed))    // ���ڵ�����,���ڱ��Ż��������������
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
            anim.SetBool("isWalking", true);
            StartWalking();
        }

        if (canAddSpeed)   // �������
        {
            Debug.Log("����");
            anim.SetBool("hasRocket", true);
            timer += Time.fixedDeltaTime;
            rb.gravityScale = 0;
            rb.velocity = new Vector3(speed * currentDirection, 0, 0);
        }
        if (timer >= 0.8f)   // ����ʱ����� 
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
    /// �����ƶ��¼������ڵ��������ߣ����ڵ�����ֱ����
    /// </summary>
    void OnMovementEvent()
    {
        isMoving = true;
        currentDirection = 1f;
    }

    /// <summary>
    /// ���������¼�������ֹͣ�ƶ����ص���ʼλ��
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
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = 0;
        rb.velocity = currentVelocity;
    }

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

        // ���� timer ����
        timer = 0f;

        //����sprite����
        Vector3 theScale = transform.localScale;
        theScale.x = Mathf.Abs(theScale.x);
        transform.localScale = theScale;
    }

    /// <summary>
    /// ת���ƶ�����
    /// </summary>
    private void FlipDirection()
    {
        // ��ת�˶�����
        currentDirection = -currentDirection;
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;
        // ��תsprite
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
            if (!physicsCheckScript.isGround)
            {
                FallDown();
                return;
            }
            float targetX = magnet.transform.position.x;
            float currentX = transform.position.x;

            // �����ƶ����� 
            currentDirection = Mathf.Sign(targetX - currentX);
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
                // �����л�����˾�ϵ���·
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
        if (collision.gameObject.CompareTag("Obstacle"))   // ��������ǽ��ض���
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
        if (collision.gameObject.CompareTag("Rocket"))   // �����������
        {
            if(!canAddSpeed)
            Debug.Log("ǰ" + canAddSpeed);
            canAddSpeed = true;
            Debug.Log("��" + canAddSpeed);
        }
        if (collision.gameObject.CompareTag("DeadZone"))   // ������䵽�߽�
        {
            OnIdleEvent();
            EventHandler.ResetEvent.Invoke();   // ���ƣ�̮���ؿ飬�������߸�λ
            EventHandler.CallTimerStopEvent();
            EventHandler.isMoving = !EventHandler.isMoving;
        }
        if (collision.gameObject.CompareTag("CheckPoint"))  // ����С�ؿ��յ�
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
    /// ����ͨ�ص㣬ֹͣ�˶�
    /// </summary>
    public void SetHorizontalVelocityZero()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
}