using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameRestart : MonoBehaviour
{
    public void RestartTutorial()
    {
        // Reset the game manager
        GameManager.ResetLevel();
        // Restart the tutorial
        SceneManager.LoadScene("Tutorial");
    }
}
