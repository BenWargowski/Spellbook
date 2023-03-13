using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void NewGame()
    {
        // Put the name of level 1 here, or the number of the scene in the load order.
        // Load the first level.
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScreen");
        // SceneManager.LoadScene("");
    }

    //public void Levelx()
    //{
    //    // load the xth level.
    //    SceneManager.LoadScene("Levelx");
    //}

    public void Level1()
    {
        SceneManager.LoadScene(1);
    }
    public void Level2()
    {
        SceneManager.LoadScene(2);
    }
    public void Level3()
    {
        SceneManager.LoadScene(3);
    }
    public void Level4()
    {
        SceneManager.LoadScene(4);
    }
    public void Level5()
    {
        // SceneManager.LoadScene("");
    }
    public void Level6()
    {
        // SceneManager.LoadScene("");
    }
    public void Level7()
    {
        // SceneManager.LoadScene("");
    }
    public void Level8()
    {
        // SceneManager.LoadScene("");
    }
    public void Level9()
    {
        // SceneManager.LoadScene("");
    }

public void Exit()
    {
        // Exit the application. Note that this will not work in the Unity editor.
        Application.Quit();
    }
}
