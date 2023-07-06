using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public void Level1()
    {
        SceneManager.LoadScene("Level1_zone1");
    }

    public void Level2()
    {
        SceneManager.LoadScene("SampleScene");
    }


    public void ReviewLevel1()
    {
        SceneManager.LoadScene("ReviewLevel1");
    }

    public void ReviewLevel2()
    {
        SceneManager.LoadScene("ReviewLevel2");
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    public void MapSelection()
    {
        SceneManager.LoadScene("MapSelectionScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
