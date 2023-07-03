using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1_zone1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
