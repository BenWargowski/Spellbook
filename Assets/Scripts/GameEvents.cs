using System;
using UnityEngine;

/// <summary>
/// Handles Events
/// </summary>
public class GameEvents : MonoBehaviour {
    //Singleton reference
    public static GameEvents Instance {get; private set;}

    private void Awake() {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    /*
        Some A-Z key has been pressed. If SHIFT (or whatever modifier key) is held down, the bool will be true.
        The key pressed will always be capitalized.
    */
    public event Action<char, bool> alphabetKeyPressed;
    public void AlphabetKeyPressed(char c, bool modifierDown) {
        if (alphabetKeyPressed != null) {
            alphabetKeyPressed(c, modifierDown);
        }
    }

    /*
        The SHIFT (or whatever modifier key) has been either pressed down or released.
        Pressed Down -> Modifier Active = TRUE
        Just Released -> Modifier Active = FALSE
    */
    public event Action<bool> modifierKeyUpdated;
    public void ModifierKeyUpdated(bool modifierActive) {
        if (modifierKeyUpdated != null) {
            modifierKeyUpdated(modifierActive);
        }
    }

    public event Action playerDeath;
    public void PlayerDeath() {
        if (playerDeath != null) {
            playerDeath();
        }
    }
    
}