using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : MonoBehaviour
{
    private ShootControl shootControl;
    public float newBallSpeed;
    private void Start()
    {
        shootControl = FindObjectOfType<ShootControl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;   
            shootControl.CreatBall(new Vector2(transform.position.x, transform.position.y),new Vector2(Random.Range(-5, 5), newBallSpeed));
            
            Destroy(gameObject);
        }
    }
}
