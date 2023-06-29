using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageClearScene : MonoBehaviour
{
    int level = 2;

    public void Setup(int levelNext)
    {
        gameObject.SetActive(true);
        this.level = levelNext;
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    public void NextLevel()
    {
        string SceneLevel = "SceneLevel" + level.ToString();
        Debug.Log(SceneLevel);
        SceneManager.LoadScene(SceneLevel);
    }
}
