using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曲线预测器
/// </summary>

public class Trajectory : MonoBehaviour
{
    /// <summary>
    /// 预测点的数量
    /// </summary>
    [SerializeField] private int m_dotsNum = 10;
    /// <summary>
    /// 点物体的父节点
    /// </summary>
    [SerializeField] private GameObject m_dotsParent;
    /// <summary>
    /// 点预设
    /// </summary>
    [SerializeField] private GameObject m_dotsPrefab;
    /// <summary>
    /// 点间距
    /// </summary>
    [SerializeField] private float m_dotSpacing = 0.01f;
    /// <summary>
    /// 点的最小缩放
    /// </summary>
    [SerializeField][Range(0.01f, 0.3f)] private float m_dotMinScale = 0.2f;
    /// <summary>
    /// 点的最大缩放
    /// </summary>
    [SerializeField][Range(0.3f, 1f)] private float m_dotMaxScale = 1f;

    /// <summary>
    /// 炮台位置
    /// </summary>
    public Transform shootHeadPos;


    private Transform[] m_dotsList;
    private Vector2 m_pos;
    private float m_timeStamp;

    private void Start()
    {
        Hide();
        PrepareDots();
    }

    /// <summary>
    /// 准备轨迹点
    /// </summary>
    private void PrepareDots()
    {
        m_dotsList = new Transform[m_dotsNum];
        m_dotsPrefab.transform.localScale = Vector3.one * m_dotMaxScale;
        float scale = m_dotMaxScale;
        float scaleFactor = scale / m_dotsNum;

        for (int i = 0; i < m_dotsNum; ++i)
        {
            var dot = Instantiate(m_dotsPrefab).transform;
            dot.parent = m_dotsParent.transform;
            dot.localScale = Vector3.one * scale;
            if (scale > m_dotMinScale)
                scale -= scaleFactor;
            m_dotsList[i] = dot;
        }
    }

    /// <summary>
    /// 更新点坐标
    /// </summary>
    /// <param name="pushSpeed">初始速度向量</param>
    public void UpdateDots(Vector2 pushSpeed)
    {
        m_timeStamp = m_dotSpacing;

        for (int i = 0; i < m_dotsNum; ++i)
        {
            m_pos.x = shootHeadPos.position.x + pushSpeed.x * m_timeStamp;
            m_pos.y = shootHeadPos.position.y + pushSpeed.y * m_timeStamp; // * Physics2D.gravity.magnitude * m_timeStamp * m_timeStamp
            m_dotsList[i].position = m_pos;
            m_timeStamp += m_dotSpacing;
        }
    }

    /// <summary>
    /// 显示预测轨迹
    /// </summary>
    public void Show()
    {
        m_dotsParent.SetActive(true);
    }

    /// <summary>
    /// 隐藏预测轨迹
    /// </summary>
    public void Hide()
    {
        m_dotsParent.SetActive(false);
        
    }
}
