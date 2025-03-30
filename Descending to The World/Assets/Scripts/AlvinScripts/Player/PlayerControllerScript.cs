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
    private float enlargeScale = 1.7f;   // sprite��scale�Ŵ���

    [Header("�ƶ�״̬")]
    [SerializeField]private bool isMoving;
    //[SerializeField] private bool isTurning = false;
    [SerializeField]private Vector3 faceDir;
    public float currentDirection;  // ��¼��ǰ�ƶ�����
    private bool canAddSpeed = false;

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

    [Header("��������")]
    public int currentBigLevel;
    public int currentSmallLevel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        physicsCheckScript = GetComponent<PhysicsCheckScript>();

        //currentBigLevel = LevelManager.Instance.currentBigLevel;
        //currentSmallLevel = LevelManager.Instance.currentSmallLevel;

        //Debug.Log("big " + currentBigLevel);
        //Debug.Log("small " + currentSmallLevel);

        //int index = ((currentBigLevel - 1) * 2 + (currentSmallLevel - 1)) + 1;
        //Debug.Log(index);
        //// �� LevelInitialSO �л�ȡ��Ӧ�����������ʼλ��
        //if (LevelManager.Instance.levelInitialData != null && index < LevelManager.Instance.levelInitialData.playerPositions.Length)
        //{
        //    initialPosition = LevelManager.Instance.levelInitialData.playerPositions[index];
        //    // �����������ڶ�Ӧ�����ĳ�ʼλ��
        //    transform.position = initialPosition;
        //}
        //else
        //    Debug.LogError("δ���ҵ���Ӧ�ĳ�ʼλ��");
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
        //Moon = GameObject.Find("Moon");
        //MilkyWay = GameObject.Find("MilkyWayCollision");
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);

        door1 = GameObject.Find("door 1");
        door2 = GameObject.Find("door 2");
        if (door1 == null && door2 == null)
            Debug.Log("�ùؿ��в����ڻ�����");
        
        // ���ĵ��ʱ�Ӵ������¼�
        EventHandler.MovementEvent.AddListener(OnMovementEvent);
        EventHandler.IdleEvent.AddListener(OnIdleEvent);
    }

    private void UpdatePlayerPosition()
    {
        currentBigLevel = LevelManager.Instance.currentBigLevel;
        currentSmallLevel = LevelManager.Instance.currentSmallLevel;

        Debug.Log("big " + currentBigLevel);
        Debug.Log("small " + currentSmallLevel);

        int index = ((currentBigLevel - 1) * 2 + (currentSmallLevel - 1)) + 1;
        Debug.Log("�ؿ���ʼλ������" + index);
        // �� LevelInitialSO �л�ȡ��Ӧ�����������ʼλ��
        if (LevelManager.Instance.levelInitialData != null && index < LevelManager.Instance.levelInitialData.playerPositions.Length)
        {
            if (this == null)
            {
                Debug.Log("ccccccccccccccccccccccccc enter");
                initialPosition = LevelManager.Instance.levelInitialData.playerPositions[index];
                // �����������ڶ�Ӧ�����ĳ�ʼλ��
                transform.position = initialPosition;
                Debug.Log("bbbbbbbbbbbbbbb set initial positon");
            }
        }
        else
            Debug.LogError("δ���ҵ���Ӧ�ĳ�ʼλ��");
    }

    private void Update()
    {
        // currentDirection = Mathf.Sign(rb.velocity.x);
        TimeCounter();
        //if (Moon.activeSelf) MilkyWay.SetActive(true);
        //else MilkyWay.SetActive(false);
    }
    private void FixedUpdate()
    {
        
        if(isMoving && physicsCheckScript.isGround && !moveToMagnet)   // �ƶ�, �ڵ�����,�������Ҵ�ʯ״̬
        {
            //currentDirection = 1f;
            anim.SetBool("isWalking", true);
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
            anim.SetBool("isWalking", true);
            StartWalking();
        }

        if (canAddSpeed)   // �������
        {
            anim.SetBool("hasRocket", true);
            timer += Time.fixedDeltaTime; rb.velocity = new Vector3(speed, 0, 0);
        }
        if (timer >= 0.8f)   // ����ʱ����� 
        {
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
        currentDirection = 1f;
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

        if (door1 != null)
            door1.SetActive(true);
        if (door2 != null)
            door2.SetActive(true);
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
        currentDirection = -currentDirection;
        // ��ת�˶�����
        //moveSpeed = -moveSpeed;
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
        //if (other.gameObject.CompareTag("DeadZone"))   // ������䵽�߽�
        //{
        //    OnIdleEvent();
        //    EventHandler.isMoving = !EventHandler.isMoving;
        //}
        //if (other.gameObject.CompareTag("CheckPoint"))  // ����С�ؿ��յ�
        //{
        //    LevelManager.Instance.OnReachCheckpoint();
        //    gameObject.SetActive(false);
        //    other.enabled = false;
        //}
        if (other.gameObject == door1 || other.gameObject == door2)
        {
            other.gameObject.SetActive(false);
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
                //FlipDirection();
            }
        }
        if (collision.gameObject.CompareTag("Rocket"))   // �����������
        {
            canAddSpeed = true;
        }
        if (collision.gameObject.CompareTag("DeadZone"))   // ������䵽�߽�
        {
            OnIdleEvent();
            EventHandler.isMoving = !EventHandler.isMoving;
        }
        if (collision.gameObject.CompareTag("CheckPoint"))  // ����С�ؿ��յ�
        {
            LevelManager.Instance.OnReachCheckpoint();
            collision.gameObject.SetActive(false);
            //collision.gameObject.enabled = false;
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