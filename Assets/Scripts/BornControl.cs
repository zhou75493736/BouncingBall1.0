using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornControl : MonoBehaviour
{

    //障碍物数量
    private static int obstacleNum = 3;
    public GameObject[] obstacleGroup = new GameObject[obstacleNum];
    public GameObject addBall;

    public GameObject obstaclesControl;
    public GameObject obstacle;

    public Vector3 moveValue = new Vector3(2, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObstacle()
    {
        int randNum = Random.Range(0, 100);
        if (randNum < 80)
        {
            obstacle = Instantiate(obstacleGroup[Random.Range(0,obstacleNum)], transform.position + moveValue, new Quaternion());
            obstacle.transform.parent = obstaclesControl.transform;
        }
        else if (randNum < 90)
        {
            obstacle = Instantiate(addBall, transform.position + moveValue, new Quaternion());
            obstacle.transform.parent = obstaclesControl.transform;
        }
        
        moveValue.x = -moveValue.x;




    }
}
