using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        WaitingToStart,
        Playing,
        GameOver
    }

    public static GameState CurrentState { get; private set; } = GameState.WaitingToStart;
    
    public static void StartGame()
    {
        CurrentState = GameState.Playing;
    }
}