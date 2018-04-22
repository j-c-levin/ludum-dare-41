using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    public void RestartGame()
    {
        // Reset the game manager
        GameManager.ResetLevel();
        // Reload the level
        SceneManager.LoadScene("Game");
    }
}
