using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialBeginGame : MonoBehaviour
{
    public GameObject music;
    public void OnCollisionEnter2D(Collision2D other)
    {
        DontDestroyOnLoad(music);
        // Remove the game end listener
        GameManager.gameEndDelegate = null;
        // Load the game
        SceneManager.LoadScene("Game");
    }
}
