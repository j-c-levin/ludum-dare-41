using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void GameEndDelegate();
    public static GameEndDelegate gameEndDelegate;
    public static bool isGameRunning
    {
        get
        {
            return _isGameRunning;
        }
        private set
        {
            _isGameRunning = value;
        }
    }
    private static bool _isGameRunning = true;

    public static void EndGame()
    {
        isGameRunning = false;
		// Call event listeners on the game ending
        if (gameEndDelegate != null)
        {
            gameEndDelegate();
        }
    }
}
