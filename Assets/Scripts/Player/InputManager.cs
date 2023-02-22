using System;
using UnityEngine;

/**
 * InputManager
 * Processes the user keyboard input and raises the appropriate events
 * for each type of input (i.e. SHIFT enabled vs. SHIFT disabled input)
 */
public class InputManager : MonoBehaviour {

    //Singleton reference (feel free to refactor this out)
    public static InputManager Instance {get; private set;}

    [SerializeField] private KeyCode modeSwitchKey = KeyCode.LeftShift; //defaults to Left SHIFT

    private void Awake() {
        //Setting singleton reference
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    //Reads the input and sends events
    private void Update() {
        StandardInput();
        ModeSwitchInput();
    }

    //Raise event with the first alphabetical character in the input string
    private void StandardInput() {
        string input = Input.inputString;
        
        foreach (char c in input) {
            if (Char.IsLetter(c)) {
                //Raise the event
                GameEvents.Instance.AlphabetKeyPressed(c, Input.GetKey(this.modeSwitchKey));
                return;
            }
        }

    }

    //This will fire when the player beigns or ends holding down SHIFT so we can start/stop animations, etc.
    private void ModeSwitchInput() {
        if (Input.GetKeyDown(modeSwitchKey)) {
            //Mode switch key just started pressing down
            GameEvents.Instance.ModifierKeyUpdated(true);
        }
        else if (Input.GetKeyUp(modeSwitchKey)) {
            //Mode switch key released
            GameEvents.Instance.ModifierKeyUpdated(false);
        }
    }

}
