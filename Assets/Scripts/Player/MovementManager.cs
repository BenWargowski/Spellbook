using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * MovementManager
 * Listens to movement key events and moves the player to the desired position.
 * Is intended to be placed on the player object
 */
public class MovementManager : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private char startingKey;

    //Accepting new key inputs
    // NOTE: Disabling this will not cause the player to immediately stop moving,
    // they just won't respond to any new movements
    public bool Active {get; set;}

    private bool isMoving;
    private Vector2 targetPosition;
    public char TargetKey {get; private set;}

    // INITIALIZATION -------
    private void Awake() {
        this.Active = true;
        this.isMoving = false;
        this.TargetKey = this.startingKey;
    }

    private void Start() {
        GameEvents.Instance.alphabetKeyPressed += OnMovementPress;

        //Character starts on the starting key -- default is Q
        this.player.transform.position = StageLayout.Instance.TilePositions[this.startingKey];
    }
    // ----------------------

    private void Update() {
        Move();

        // Update animator variables
        if (isMoving == true)
        { animator.SetInteger("isMoving", 1); }
        else
        { animator.SetInteger("isMoving", 0); }
    }

    /// <summary>
    /// Listens to a movement keypress and updates the target position
    /// </summary>
    /// <param name="c">Character pressed on the keyboard</param>
    public void OnMovementPress(char c, bool shiftKey) {
        // truth table
        // lowercase | shift held || caps lock enabled | mode
        // F         | F          || F                 | move
        // F         | T          || F                 | spell
        // T         | F          || T                 | spell
        // T         | T          || T                 | spell

        //guard clauses -- not shift-modified and key actually exists
        if (Char.IsUpper(c) || shiftKey) return;
        c = Char.ToUpper(c);

        if (!this.Active) return;
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        //move to the key 
        this.targetPosition = StageLayout.Instance.GetTilePosition(c);
        this.TargetKey = c;
        this.isMoving = true;
    }

    /// <summary>
    /// Main movement mechanic, moves the player towards the target position each frame
    /// </summary>
    private void Move() {
        if (!this.isMoving) return;

        float step = this.player.MovementSpeed * Time.deltaTime;
        this.player.transform.position = Vector2.MoveTowards(
            this.player.transform.position,
            this.targetPosition,
            step
        );

        if (Vector2.Distance(this.player.transform.position, this.targetPosition) <= 0.01) {
            this.isMoving = false;
        }
    }
}
