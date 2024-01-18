using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("Video Player");
    }

    public void Quit()
    {
       Application.Quit();
    }
}
