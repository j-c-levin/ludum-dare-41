using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBeginGame : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D other)
    {
        // Remove the game end listener
        GameManager.gameEndDelegate = null;
        // Load the game
        SceneManager.LoadScene("Game");
    }
}
