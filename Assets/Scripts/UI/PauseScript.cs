using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button quitButton;  
    private bool isPaused = false;

    void Start()
    {
        resumeButton.onClick.AddListener(resume);
        quitButton.onClick.AddListener(quitClick);
        pauseMenu.SetActive(false);
    }

    void resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenu.SetActive(false);
    }
    void quitClick()
    {
        SceneManager.LoadScene("StartScreen");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0f;
                isPaused = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                resume();
            }
        }
    }
}