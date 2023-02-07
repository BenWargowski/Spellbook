using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * BehaviorStateManager
 * Manages current behavior state of enemy
 */
public class BehaviorStateManager : MonoBehaviour
{
    [SerializeField] private BehaviorState currentState;

    [SerializeField] private Transform target; // may change to some player script later

    [field: SerializeField] public float DefaultSpeed { get; private set; }

    private EnemyMovementManager movement;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform; // may change with more clean code later

        movement = GetComponent<EnemyMovementManager>();
    }

    void Start()
    {
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnStateTriggerEnter(this, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnStateTriggerExit(this, other);
    }

    /// <summary>
    /// Change BehaviorStateManager's current state to newState
    /// </summary>
    public void ChangeState(BehaviorState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
    
    /// <summary>
    /// Returns position of target (Player)
    /// </summary>
    public Vector2 GetTargetPosition()
    {
        return target != null ? new Vector2(target.position.x, target.position.y) : Vector2.zero;
    }

    /// <summary>
    /// Sets target position for enemy movement
    /// </summary>
    /// <param name="tileKey">Character that corresponds to some tileKey on the level</param>
    /// <param name="speed">How fast the enemy will be moving</param>
    public void SetMovement(char tileKey, float speed)
    {
        movement.SetTargetPosition(tileKey, speed);
    }

    /// <summary>
    /// Returns bool on whether or not enemy has reached its target position
    /// </summary>
    public bool GetIsMoving()
    {
        return movement.GetIsMoving();
    }

    /// <summary>
    /// Returns bool on whether or not enemy is facing right
    /// </summary>
    public bool GetIsFacingRight()
    {
        if (GetIsMoving())
        {
            return movement.GetIsFacingRight();
        }
        else
        {
            return GetTargetPosition().x - transform.position.x > 0;
        }
    }
}
