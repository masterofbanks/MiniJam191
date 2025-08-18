using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
