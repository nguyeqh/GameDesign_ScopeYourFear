using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void Setup(int points)
    {
        gameObject.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene"); //GameLevel2Zone1_Scene
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScreen");
    }

   
}
