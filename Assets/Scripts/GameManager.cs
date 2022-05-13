using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Start = 0,
        GameOver = 1,
    }
    static GameState state;

    void Start()
    {
        state = GameState.Start;
    }
    
    public static void SetGameState(GameState gameState)
    {
        state = gameState;
    }

    void Update()
    {
        if (state == GameState.GameOver)
        {
            Debug.Log("GAME OVER");
        }   
    }
}
