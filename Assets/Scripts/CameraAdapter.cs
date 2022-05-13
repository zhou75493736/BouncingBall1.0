using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraAdapter : MonoBehaviour
{
    const float devHeight = 9.6f;
    const float devWidth = 6.4f;

    public Transform environment;

    public Transform artillery;

    public Transform burnPoint;
    public static int backPointNum = 5;
    public Transform[] backPoint = new Transform[backPointNum];

   void Start()
    {
        float screenHeight = Screen.height; //获取屏幕尺寸
        //Debug.Log("screenHeight = " + screenHeight);
        float orthographicSize = this.GetComponent<Camera>().orthographicSize; // 拿到相机的正交属性设置摄像机大小

        float aspectRotio = Screen.width * 1.0f / Screen.height; //得到宽高比
        
        //实际的宽高比和摄像机的orthographicSize值来计算出摄像机的宽度值
        float cameraWidth = orthographicSize * 2 * aspectRotio;
        //Debug.Log("cameraWidth = " + cameraWidth);
        float objectScale = (1 - aspectRotio) * 2.6f;  //缩放比例

        //Debug.Log(objectScale);

        if (cameraWidth < devWidth)  //如果摄像机宽度小于设计尺寸宽度
        {
            environment.localScale = new Vector3(1, objectScale, 1); //场景缩放
            //游戏对象位置缩放
            artillery.position = new Vector3(artillery.position.x, artillery.position.y * objectScale, artillery.position.z);
            
            for (int i = 0; i < burnPoint.childCount; i++)
            {
                burnPoint.GetChild(i).transform.position = new Vector3(burnPoint.GetChild(i).transform.position.x, burnPoint.GetChild(i).transform.position.y * objectScale, burnPoint.GetChild(i).transform.position.z);
            }
            for(int i = 0; i < backPointNum; i++)
            {
                backPoint[i].position = new Vector3(backPoint[i].position.x, backPoint[i].position.y * objectScale, backPoint[i].position.z);
            }
            orthographicSize = devWidth / (2 * aspectRotio); // 将尺寸宽度 /2倍的宽高比 = 相机的大小
            //Debug.Log("new orthographicSize = " + orthographicSize);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;   // 将这个摄像机大小赋值回摄像机属性
        }
    }
}