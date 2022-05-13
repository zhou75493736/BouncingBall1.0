using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{
    public Vector3 moveDestance = new Vector3(0, 3, 0);

    public Transform topLine;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveUp()
    {
        
        transform.position += moveDestance;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.position.y > topLine.position.y)
            {
                GameManager.SetGameState(GameManager.GameState.GameOver);
            }
        }
    }
}
