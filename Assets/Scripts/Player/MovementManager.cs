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

    public Animator animator;
    public int walkDirection;

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

        // Update animator variables:
        // isMoving - whether the player is currently walking (0=no, 1=yes)
        if (isMoving == true) { animator.SetInteger("isMoving", 1); }
        else { animator.SetInteger("isMoving", 0); }
        // walkDirection - which direction the player is walking in (0=left, 1=up, 2=right, 3=down)
        if (isMoving == true) { animator.SetInteger("walkDirection", walkDirection); }
        
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

        //used for determining which walk animation is used based on the furthest distance from current location
        walkDirection = getWalkDirection((transform.position.x - targetPosition.x), (transform.position.y - targetPosition.y));
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

    // determine which cardinal direction the player is moving the most toward (0=left, 1=up, 2=right, 3=down)
    public int getWalkDirection(double x, double y)
    {
        int walkDirection = 0;

        
        bool isNegativeX = false;
        bool isNegativeY = false;

        if (x < 0) { x *= -1; isNegativeX = true; }
        if (y < 0) { y *= -1; isNegativeY = true; }

        if (x > y)
        {
            if (isNegativeX == false) { walkDirection = 0; }
            else { walkDirection = 2; }
        }
        else if (y > x)
        {
            if (isNegativeY == false) { walkDirection = 3; }
            else { walkDirection = 1; }
        }

        //DELTE THESE 2 LINES ONCE LEFT AND RIGHT HAVE BEEN ADDED
        if (isNegativeY == false) { walkDirection = 3; }
        else { walkDirection = 1; }
        //

        return walkDirection;
    }
}
