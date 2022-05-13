using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    private GameObject shoot;
    public GameObject text;
    private int num;

    [SerializeField][Range(1, 3)] int min = 5;
    [SerializeField][Range(3, 5)] int max = 20;

    void Start()
    {
        float angle = Random.Range(0, 360);
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, max);
        shoot = GameObject.FindWithTag("Shoot");
        SetTextActive();
        RandomNum(shoot.GetComponent<ShootControl>().ballNum);
        SetText();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball"){
            num--;
            SetText();
        }
        if (num <= 0){
            Destroy(this.gameObject);
        }
    }

    void SetTextActive(){
        text.SetActive(true);
    }

    void SetText(){
            text.GetComponent<Text>().text = num.ToString();
    }
    void RandomNum(int ballNum){
        int minNum = ballNum * min;
        int maxNum = ballNum * max;
        num = Random.Range(minNum, maxNum);
    }

  

}
