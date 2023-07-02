using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class StageClearScene : MonoBehaviour
{
    public GameObject successPannel, player;
    public string nextStage;

    private bool stage_finish = false;

    void Update()
    {
        stage_finish = player.GetComponent<CharacterMovement>().stage_finish;
        if (stage_finish)
        {
            Setup();
        }
    }


    public void Setup()
    {
        successPannel.SetActive(true);
    
    }

    public void BackToHome()
    {
        Debug.Log("Back to Home button clicked!");
        SceneManager.LoadScene("HomeScreen");
    }

    public void NextLevel()
    {
        try
        {
            Debug.Log(nextStage);
            SceneManager.LoadScene(nextStage);
        }
        catch (SceneNotFoundException)
        {
            Debug.LogError("Scene not found: " + nextStage);
            // Perform alternative action or display error message
        }
        
    }
}

public class SceneNotFoundException : Exception
{
    public SceneNotFoundException() { }

    public SceneNotFoundException(string message)
        : base(message) { }

    public SceneNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
