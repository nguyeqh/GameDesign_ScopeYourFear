using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public GameObject deathPannel, player;
    public string currentStage;

    private bool charDie = false;

    void Update()
    {
        charDie = player.GetComponent<CharacterMovement>().characterIsDead;
        if (charDie)
        {
            Setup();
        }
    }

    public void Setup()
    {
        deathPannel.SetActive(true);

    }


    public void Restart()
    {
        SceneManager.LoadScene(currentStage); //GameLevel2Zone1_Scene
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScreen");
    }

   
}
