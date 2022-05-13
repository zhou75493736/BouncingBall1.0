using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    /// <summary>
    /// 小球返回的路径点
    /// </summary>
    private Transform leftPoint1;
    private Transform leftPoint2;
    private Transform rightPoint1;
    private Transform rightPoint2;
    private Transform endPoint;

    private Vector3 point1Pos;
    private Vector3 point2Pos;


    public enum BallState
    {
        Shooting = 1,
        Back1 = 2,
        Back2 = 3,
        Back3 = 4,
    }

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public CircleCollider2D col;

    public BallState ballState;

    private float checkTime;
    //判断球是否静止的速度
    private float lastPosX;
    private float lastPosY;
    //小球返回时速度
    public float backSpeed = 4;

    public float addSpeed;


    public Vector3 pos
    {
        get { return transform.position; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }



    // Start is called before the first frame update
    void Start()
    {
        leftPoint1 = GameObject.Find("LeftPoint1").transform;
        leftPoint2 = GameObject.Find("LeftPoint2").transform;
        rightPoint1 = GameObject.Find("RightPoint1").transform;
        rightPoint2 = GameObject.Find("RightPoint2").transform;
        endPoint = GameObject.Find("EndPoint").transform;
        ballState = BallState.Shooting;
        checkTime = Time.time;
        lastPosX = transform.position.x;
        lastPosY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        BallBackState();
        if (ballState >= BallState.Back1)
        {
            BallBack();
        }
        if (ballState == BallState.Shooting)
        {
            StayObstacle();
        }
    }

    /// <summary>
    /// 给球一个推力，将球推出去
    /// </summary>
    /// <param name="speed">速度向量</param>
    public void Push(Vector2 speed)
    {
        rb.AddForce(speed, ForceMode2D.Impulse);
        

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "DownLine")
        {
            IsLeftOrRight();
        }

        
        
    }

   

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            rb.AddForce(rb.velocity);
            

        }

        if (collision.gameObject.tag == "Floor")
        {
            DesActivateRb();
            ballState = BallState.Back1;
        }

       
    }
    /// <summary>
    /// 判断小球是否静止，在方块间卡住
    /// </summary>
    void StayObstacle()
    {
        // Debug.Log(checkTime);
        //判断是否有移动
        if (Time.time - checkTime > 0.5f)
        {

            if (transform.position.x == lastPosX && transform.position.y == lastPosY)
            {
                
                rb.AddForce(new Vector2(Random.Range(-10, 10), addSpeed) * Time.deltaTime, ForceMode2D.Impulse);
            }
            lastPosX = transform.position.x;
            lastPosY = transform.position.y;
            Debug.Log(lastPosX);
            Debug.Log(lastPosY);
            checkTime = Time.time;
        }

    }
 
    /// <summary>
    /// 禁用物理组件
    /// </summary>
    void DesActivateRb()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
    }

    /// <summary>
    /// 小球返回路径
    /// </summary>
    public void BallBack()
    {
        if (ballState == BallState.Back1)
        {
            transform.position = Vector3.MoveTowards(transform.position, point1Pos, backSpeed * Time.deltaTime);
            
        }
        if (ballState == BallState.Back2)
        {
            transform.position = Vector3.MoveTowards(transform.position, point2Pos, backSpeed * Time.deltaTime);
        }
        if (ballState == BallState.Back3)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, backSpeed * Time.deltaTime);
        }
       

    }
    /// <summary>
    /// 球返回时状态
    /// </summary>
    void BallBackState()
    {
        if (ballState == BallState.Back1 && (Mathf.Abs(transform.position.x - point1Pos.x)) < 0.1)
        {
            ballState = BallState.Back2;
        }
        if (ballState == BallState.Back2 && (Mathf.Abs(transform.position.y - point2Pos.y)) < 0.1)
        {
            ballState = BallState.Back3;
        }
        
    }

   
    /// <summary>
    /// 判断球落下后在左边还是右边
    /// </summary>
    void IsLeftOrRight()
    {
        if (transform.position.x < 0)
        {
            point1Pos = leftPoint1.position;
            point2Pos = leftPoint2.position;
            
        }
        else
        {
            point1Pos = rightPoint1.position;
            point2Pos = rightPoint2.position;
        }
    }
}
