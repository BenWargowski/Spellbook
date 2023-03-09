using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Button button;
    [SerializeField]
    private int totalWalkCount;
    [SerializeField]
    private TextMeshProUGUI tutorialText;

    private int mode; // 0 for walk, 1 for casting
    private int walkCount;
    private char currTile;


    void Start()
    {
        mode = 0;
        walkCount = 0;
        setRandTile();
    }


    void Update()
    {
        setTutorialText();

        if (mode == 0) {
            walkMode();
        }
        else {
            castingMode();
        }
    }


    // Switches modes between walkMode (0) and castingMode (1).
    public void toggleMode() {
        mode = 1 - mode;
    }


    // While in walkMode (mode == 0), this function will highlight a new tile everytime the player walks toward the current tile.
    private void walkMode() {
        highlightTile(currTile);

        if (player.TargetKey == currTile) {
            walkCount++;
            setRandTile();
        }

        if (walkCount == totalWalkCount) {
            button.gameObject.SetActive(true);
        }
    }


    // Currently does nothing.
    private void castingMode() {

    }


    // Sets the tutorial text to the appropriate string depending on the mode.
    private void setTutorialText() {
        if (mode == 0) {
            int strWalkCount = Mathf.Min(walkCount, totalWalkCount); // Just makes sures the presented walkCount is never greater than totalWalkCount.

            //Not completed
            if (walkCount < totalWalkCount) {
                tutorialText.text = String.Format("Move to blinking key by pressing\ncorresponding key on keyboard.\n{0} / {1}", strWalkCount, totalWalkCount);
            }
            //Completed
            else {
                tutorialText.text = "You have completed the Movement tutorial. You can keep practicing movement, or continue to Spellcasting.";
            }

        }
        else {
            tutorialText.text = "To cast a spell: hold shift, type in the name of a spell, and press enter.\nDefeat the Dummy Boss to move on.";
        }
    }


    // Sets the current tile to a random tile in the ascii range 'A' (65) - 'Z' (90). Cannot be the same tile the player is currently on.
    private void setRandTile() {
        do {
            currTile = (char)UnityEngine.Random.Range(65, 90);
        } while (player.TargetKey == currTile);
    }


    // "Highlights" a tile by getting it's animator and setting the trigger to "Blink."
    private void highlightTile(char c) {
        GameObject tile = StageLayout.Instance.Tiles[c];
        Tile tileData = null;
        tile.TryGetComponent<Tile>(out tileData);
        Animator animator = null;
        tileData.Sprite.TryGetComponent<Animator>(out animator);
        animator.SetTrigger("Blink");
    }
}
