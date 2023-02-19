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
        SceneManager.LoadScene(0);
    }

    // Continue to the next boss
    public void Continue()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene((buildIndex >= SceneManager.sceneCountInBuildSettings - 1 ? 0 : buildIndex + 1), LoadSceneMode.Single);
    }
}
