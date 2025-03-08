using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 initialPosition;
    public bool isMoving;

    private Rigidbody2D rb;
    private Animator anim;
    private PhysicsCheck physicsCheck;

    //private float originalSpeed;

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
        // �ƶ������ڵ�����
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
    /// Player�ƶ�ʱ���ʱ�ӣ��ص���ʼλ��
    /// </summary>
    public void BackToInitial()
    {
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
    }
}