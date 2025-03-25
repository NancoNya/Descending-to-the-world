using Unity.VisualScripting;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("AlvinScript")]
    private float speed = 8.0f;  // ���Ͳ�ٶ�
    private float timer = 0f;
    private bool canAddSpeed = false;
    private GameObject Moon;
    private GameObject MilkyWay;

    [Header("��ʼ����")]
    public float moveSpeed;
    public Vector3 initialPosition;
    private int enlargeScale = 5;   // sprite��scale�Ŵ���

    [Header("״̬")]
    public bool isMoving;
    public Vector3 faceDir;
    public bool hasCompass = false;  // �Ƿ����Ŵ�ʯ
    public bool arriveMagnet = false;
    // public bool afterArriveMagnet = false;
    public bool moveToMagnet = false;  // �Ƿ��������ʯ�ƶ�
    [SerializeField]private float currentDirection; // ��¼���ʯ�ƶ�ʱ���ƶ�����

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
        // �ƶ�, �ڵ�����,δ�����ʯ״̬
        if(isMoving && physicsCheckScript.isGround && !arriveMagnet)
        {
            currentDirection = 1f;  // PS: ������������ͼƬ��С����
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

        // ��������д��ڴ�ʯ�������ʯ�ƶ�
        if (hasCompass && moveToMagnet) 
        {
            MoveTowardsMagnet();  // PS��������������PickUpCompass()��ִ��
        }

        // �����ʯ�󣬰���ǰ�ƶ���������ƶ�
        if (isMoving && physicsCheckScript.isGround && arriveMagnet)
        {
            WalkAnim();
            // rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);
            StartWalking();
        }

        // �������
        if (canAddSpeed) 
        {
            Debug.Log("������" + rb.gravityScale);
            timer += Time.fixedDeltaTime; rb.velocity = new Vector3(speed, 0, 0);
            // TODO: ���ű��������
        }
        if (timer >= 0.8f)
        {
            rb.velocity = new Vector2(moveSpeed,0); 
            canAddSpeed = false;
            rb.gravityScale = 3f;
            // TODO: ������·����
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
        currentDirection = 1f;  // PS: ������������ͼƬ��С����
        canAddSpeed = false;
        isMoving = false;
        hasCompass = false;
        // arriveMagnet = false;
        //afterArriveMagnet = false;
        moveToMagnet = false;
        ////////////////////anim.SetBool("hasCompass", false);
        IdleAnim();
        BackToInitial();
    }


    /// <summary>
    /// ���ʱ�ӣ�����ƶ��ٶ�
    /// </summary>
    public void StartWalking()
    {
            rb.velocity = new Vector2(currentDirection * moveSpeed, 0f);  // PS: ������������ͼƬ��С����
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
        ///anim.SetBool("hasCompass",false);
    }

    public void IdleAnim()
    {
        ///////////anim.SetBool("isWalking", false);
        //////anim.SetBool("hasCompass",false);
        //Debug.Log("idle");
    }

    public void CompassAnim()
    {
        // anim.SetBool("hasCompass",true);
        // anim.SetBool("isWalking", false);
    }
    /// <summary>
    /// Player�ƶ�ʱ���ʱ�ӣ��ص���ʼλ��
    /// </summary>
    public void BackToInitial()
    {
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
        Debug.Log(moveSpeed);
        moveSpeed = -moveSpeed;
        Debug.Log(moveSpeed);
        Vector2 currentVelocity = rb.velocity;
        currentVelocity.x = moveSpeed;
        rb.velocity = currentVelocity;
        isFacingRight = !isFacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    /// <summary>
    /// ����sprite��ת
    /// </summary>
    //private void FlipSprite()
    //{
    //    Vector3 theScale = transform.localScale;
    //    theScale.x *= -1;
    //    transform.localScale = theScale;
    //}

    /// <summary>
    /// ʰȡ˾�ϣ����ҳ��������޴�ʯ
    /// </summary>
    public void PickUpCompass()
    {
        hasCompass = true;
        ///////CompassAnim();
        // FindMagnet();
        // ʰȡ��Ѱ�Ҵ�ʯ
        magnet = GameObject.FindWithTag("Magnet");
        if (magnet != null)
        {
            //hasCompass = true;
            moveToMagnet = true;
        }
    }

    /// <summary>
    /// ʰȡ˾�Ϻ�Ѱ�Ҵ�ʯ��
    /// </summary>
    //private void FindMagnet()
    //{
    //    magnet = GameObject.FindWithTag("Magnet");
    //    if (magnet != null)
    //    {
    //        //hasCompass = true;
    //        moveToMagnet = true;
    //    }
    //}

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
            //float direction = (Mathf.Sign(targetX - currentX))*5;

            // �����ƶ����� 
            currentDirection = Mathf.Sign(targetX - currentX);  // PS: ������������ͼƬ��С����

            // ����sprite����
            if (currentDirection != 0)
            {
                Vector3 theScale = transform.localScale;
                theScale.x = currentDirection * enlargeScale;
                transform.localScale = theScale;
                //transform.localScale = new Vector3(currentDirection * enlargeScale, enlargeScale, 1f);  // PS: ������������ͼƬ��С����
            }
            //// ����sprite����
            //if (currentDirection != 0)
            //{
            //    transform.localScale = new Vector3(direction, 5f, 1f);
            //}
            // �����µ�λ��
            float newX = Mathf.MoveTowards(currentX, targetX, moveSpeed * Time.deltaTime);
            rb.MovePosition(new Vector2(newX, rb.position.y));
            // �ж��Ƿ񵽴�Ŀ��λ��
            if (Mathf.Abs(currentX - targetX) < positionThreshold)
            {
                //Debug.Log("arrive");
                // ����Ŀ��λ�ã�ֹͣ�ƶ������Ŵ�������
                // rb.velocity = Vector2.zero;
                ////////////////////////////anim.SetBool("hasCompass", false);
                ////////////////////////////anim.SetBool("isWalking", false);
                hasCompass = false;
                // TODO �����л�����˾�ϵ���·
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