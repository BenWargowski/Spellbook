using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages current behavior state of enemy
/// </summary>
public class BehaviorStateManager : MonoBehaviour
{
    [SerializeField] private BehaviorState currentState;

    [SerializeField] private Transform target;

    [field: SerializeField] public float DefaultSpeed { get; private set; }

    [SerializeField] private BehaviorState deathState;

    [SerializeField] private BehaviorState idleState;

    private EnemyMovementManager movement;

    void Awake()
    {
        target = GameObject.FindWithTag("Player")?.transform; // may change with more clean code later

        movement = GetComponent<EnemyMovementManager>();
    }

    void Start()
    {
        currentState.EnterState(this);

        GameEvents.Instance.playerDeath += PlayerDefeat;
        GameEvents.Instance.playerVictory += EnemyDeath;
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
        currentState.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    private void PlayerDefeat()
    {
        ChangeState(idleState);
    }

    private void EnemyDeath()
    {
        movement.ResetTargetPosition();

        ChangeState(deathState);
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
