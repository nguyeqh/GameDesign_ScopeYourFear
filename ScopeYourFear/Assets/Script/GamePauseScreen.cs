using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseScreen : MonoBehaviour
{
    public GameObject pausePannel;
    private bool gamePausing = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            gamePausing = true;
        }

        if (gamePausing)
        {
            pausePannel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    
    public void Continute()
    {
        pausePannel.SetActive(false);
  
        gamePausing = false;
        Time.timeScale = 1;
    }

    public void BackToHome()
    {
        pausePannel.SetActive(false);
        SceneManager.LoadScene("HomeScreen");
    }

    public void Setting()
    {
        //do something like load setting screen

    }

   
}
