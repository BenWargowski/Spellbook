using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] private GameObject victoryScreen;

    void Start()
    {
        victoryScreen.SetActive(false);
        GameEvents.Instance.playerVictory += Victory;
    }

    private void Victory()
    {
        victoryScreen.SetActive(true);
    }
    
    public void ReturnToMainMenu()
    {
        // main menu not yet implemented
    }

    // Continue to the next boss
    public void Continue()
    {
        // another boss not yet implemented
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single); // will remove later
    }
}
