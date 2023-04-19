using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
    // Method will be called when the Start Button is pressed and will load next scene in scene index
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Method will be called when user presses quit button in MainMenu Scene.
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit Game");
    }
}
