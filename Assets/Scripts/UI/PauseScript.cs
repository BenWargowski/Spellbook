using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false;

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
                Time.timeScale = 1f;
                isPaused = false;
                pauseMenu.SetActive(false);
            }
        }
    }
}