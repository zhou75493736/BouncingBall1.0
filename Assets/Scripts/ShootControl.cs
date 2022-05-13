using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootControl : MonoBehaviour
{
    /// <summary>
    /// 障碍物出生点数量
    /// </summary>
    private static int bornPointNum = 6;
    public BornControl[] bornControl = new BornControl[bornPointNum];
    /// <summary>
    /// Ready 准备状态
    /// Shooting 瞄准状态
    /// Wait 发射后等待状态
    /// End 小球全部落入地面
    /// </summary>
    enum ShootState
    {
        Ready = 0,
        Shooting = 1,
        Wait = 2,
        End = 3,
    }
    /// <summary>
    /// 轨迹预测器
    /// </summary>
    public Trajectory trajectory;

    /// <summary>
    /// 主摄像机
    /// </summary>
    private Camera m_cam;

    /// <summary>
    /// 力大小
    /// </summary>
    [SerializeField]
    private float m_speedFactor = 4f;

    /// <summary>
    /// 手指的起始点
    /// </summary>
    private Vector2 m_startPoint;
    /// <summary>
    /// 手指的结束点
    /// </summary>
    private Vector2 m_endPoint;
    /// <summary>
    /// 起始点和结束点的距离
    /// </summary>
    private float m_distance;
    /// <summary>
    /// 方向向量，从结束点指向起始点的归一化向量
    /// </summary>
    private Vector2 m_direction;
    /// <summary>
    /// 力向量
    /// </summary>
    private Vector2 m_pushSpeed;

    /// <summary>
    /// 球预制件
    /// </summary>
    public GameObject ball;

    /// <summary>
    /// 克隆出来的球
    /// </summary>
    private GameObject newBall;

    /// <summary>
    /// 球的数量
    /// </summary>
    public int ballNum = 1;

    /// <summary>
    /// 已经发射球的数量
    /// </summary>
    private int shootedBallNum;

    private ShootState shootState;

    private Vector2 direction;

    /// <summary>
    /// 旋转速度
    /// </summary>
    public float rotationSpeed;

    public Transform shootHeadPos;

    public Transform shootEndPos;

    private Vector2 shootHeadPoint;

    private Vector2 shootEndPoint;
    /// <summary>
    /// 小球发射延迟时间
    /// </summary>
    public float delayTime = 0.1f;

    private float latetime;

    public ObstacleControl obstacleControl;



    /// <summary>
    /// 用来读取过线小球的数量
    /// </summary>
    public GameObject deadTrigger;
    [HideInInspector]
    public int passNum;

    
    public GameObject ballCount;

    public float angleLeftLimit;
    public float angleRightLimit;

    // Start is called before the first frame update
    void Start()
    {
        shootedBallNum = 0;
        shootState = ShootState.Ready;
        m_cam = Camera.main;
        latetime = Time.time;
        //游戏开始时生成障碍物
        for (int i = 0; i < bornPointNum; i++)
        {
            bornControl[i].CreateObstacle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (shootState <= ShootState.Shooting)
        //{
        //    ShootRotate();
        //}
        if (shootState == ShootState.Ready)
        {
            // 检测鼠标/手指行为
            if (Input.GetMouseButtonDown(0))
            {
                OnDragStart();
            }
            if (Input.GetMouseButtonUp(0))
            {
                OnDragEnd();
                
            }
            OnDrag();

            ShootRotate();
        }
        if (shootState == ShootState.Shooting)
        {
            DelayShoot();
        }

        
        if(shootState == ShootState.Wait)
        {
            isReloadShoot();
        }
    }
    /// <summary>
    /// 开始拉
    /// </summary>
    private void OnDragStart()
    {
        // 起始点
        m_startPoint = m_cam.ScreenToWorldPoint(Input.mousePosition);
        // 显示轨迹
        trajectory.Show();
    }

    /// <summary>
    /// 拉中
    /// </summary>
    private void OnDrag()
    {
        m_endPoint = m_cam.ScreenToWorldPoint(Input.mousePosition);
        m_distance = Vector2.Distance(m_startPoint, m_endPoint);    //鼠标拖动距离
        if (m_distance > 3f)
        {
            m_distance = 3f;
        }
        shootEndPoint = new Vector2(shootEndPos.position.x, shootEndPos.position.y);
        shootHeadPoint = new Vector2(shootHeadPos.position.x, shootHeadPos.position.y);
        m_direction = (shootEndPoint - shootHeadPoint).normalized;
        m_pushSpeed = m_direction * m_distance * m_speedFactor;
        trajectory.UpdateDots(m_pushSpeed * 2);

    }

    /// <summary>
    /// 拉结束
    /// </summary>
    private void OnDragEnd()
    {
        shootState = ShootState.Shooting;
        // 隐藏轨迹
        trajectory.Hide();
    }


    /// <summary>
    /// 创建Ball克隆体
    /// </summary>
    /// <returns>Ball的gameobject</returns>
    public void PushBall()
    {
        CreatBall(new Vector2(shootHeadPos.position.x, shootHeadPos.position.y), m_pushSpeed);

    }
    public void CreatBall(Vector2 shootingPos,Vector2 pushSpeed)
    {

        newBall = Instantiate(ball, shootingPos, new Quaternion());
        newBall.transform.parent = ballCount.transform;
        newBall.GetComponent<Ball>().Push(pushSpeed / 2.5f);
        
    }
    /// <summary>
    /// 炮台旋转
    /// </summary>
    void ShootRotate(){
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
        //Debug.Log(angle);
        if (angle <= angleLeftLimit || (angle > 180 && angle <= 270))
        {
            angle = angleLeftLimit;
        }

        if (angle >= angleRightLimit && angle < 180)
        {
            angle = angleRightLimit;
        }
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
    /// <summary>
    /// 延迟发射小球
    /// </summary>
    void DelayShoot()
    {
        if ((Time.time - latetime) > delayTime && shootedBallNum < ballNum)
        {
            PushBall();
            latetime = Time.time;
            shootedBallNum++;
        }
        if(shootedBallNum == ballNum)
        {
            shootState = ShootState.Wait;
        }
    }

    void isReloadShoot()
    {
        if (ballCount.transform.childCount <= 0)
        {
            ballNum = deadTrigger.GetComponent<DeadTrigger>().passBallNum;

            deadTrigger.GetComponent<DeadTrigger>().passBallNum = 0;
            shootedBallNum = 0;
            shootState = ShootState.Ready;
            obstacleControl.MoveUp();
            for (int i = 0; i < bornPointNum; i++)
            {
                    bornControl[i].CreateObstacle();
            }
        }
    }
}
