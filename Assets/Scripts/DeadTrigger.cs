using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTrigger : MonoBehaviour
{
    public int passBallNum;
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            passBallNum++; 
            Destroy(coll.gameObject);
        }
    }
}
