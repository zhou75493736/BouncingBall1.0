using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObstacle : MonoBehaviour
{
    private GameObject target;
    public bool alwaysFollow = true;

    bool hasFollowed = false;
    private Canvas canvas;


    void Start()
    {
        target = transform.parent.parent.gameObject;
        canvas = transform.parent.GetComponent<Canvas>();
    }
    public void Init()
    {
        FollowObject();
    }

    public void Update()
    {
        FollowObject();
    }
    /// <summary>
    /// 文本跟随物体移动
    /// </summary>
    void FollowObject()
    {
        if (!alwaysFollow && hasFollowed)
            return;

        if (target != null)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(target.transform.position);
            Vector2 point;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, pos, canvas.worldCamera, out point))
            {
                transform.localPosition = new Vector3(point.x, point.y, 0);
                hasFollowed = true;
            }
        }
    }

}
