using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the screen that pops up when the player dies.
/// Listens to the PlayerDeath event
/// </summary>
public class DeathScreen : MonoBehaviour {
    [Header("References")]
    [SerializeField] private GameObject deathScreen;

    private void Start() {
        this.deathScreen.SetActive(false);
        GameEvents.Instance.playerDeath += Death;
    }

    private void Death() {
        this.deathScreen.SetActive(true);
    }

    //TODO: might not a proper way to reload the game
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

}
