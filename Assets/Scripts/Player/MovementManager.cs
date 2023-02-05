using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * MovementManager
 * Listens to movement key events and moves the player to the desired position.
 * Is intended to be placed on the player object
 */
public class MovementManager : MonoBehaviour {
    [Header("Settings")]
    [SerializeField] private int moveSpeed;
    [SerializeField] private char startingKey;

    //Accepting new key inputs
    // NOTE: Disabling this will not cause the player to immediately stop moving,
    // they just won't respond to any new movements
    public bool Active {get; set;}

    private bool isMoving;
    private Vector2 targetPosition;

    // INITIALIZATION -------
    private void Awake() {
        this.Active = true;
        this.isMoving = false;
    }

    private void Start() {
        GameEvents.Instance.alphabetKeyPressed += OnMovementPress;

        //Character starts on the starting key -- default is Q
        transform.position = StageLayout.Instance.TilePositions[this.startingKey];
    }
    // ----------------------

    private void Update() {
        Move();
    }

    /// <summary>
    /// Listens to a movement keypress and updates the target position
    /// </summary>
    /// <param name="c">Character pressed on the keyboard</param>
    public void OnMovementPress(char c, bool shiftKey) {
        //guard clauses -- not shift-modified and key actually exists
        if (shiftKey) return;
        if (!this.Active) return;
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        //move to the key 
        this.targetPosition = StageLayout.Instance.TilePositions[c];
        this.isMoving = true;
    }

    /// <summary>
    /// Main movement mechanic, moves the player towards the target position each frame
    /// </summary>
    private void Move() {
        if (!this.isMoving) return;

        float step = this.moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(
            transform.position,
            this.targetPosition,
            step
        );

        if (Vector2.Distance(transform.position, this.targetPosition) <= 0.01) {
            this.isMoving = false;
        }
    }
}
