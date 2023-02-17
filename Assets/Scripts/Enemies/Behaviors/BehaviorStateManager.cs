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

    [SerializeField] private BehaviorState stunnedState;

    [SerializeField] private BehaviorState recoveredState;

    private EnemyStatusManager statusManager;

    private EnemyMovementManager movementManager;

    private Animator animator;

    void Awake()
    {
        target = GameObject.FindWithTag("Player")?.transform; // may change with more clean code later

        statusManager = GetComponent<EnemyStatusManager>();
        statusManager.onStunned += Stunned;
        statusManager.onNotStunned += StunRecovery;

        movementManager = GetComponent<EnemyMovementManager>();

        animator = GetComponentInChildren<Animator>();
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

    void OnTriggerStay2D(Collider2D other)
    {
        currentState.OnStateTriggerStay(this, other);
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
        movementManager.ResetTargetPosition();

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
    /// <param name="target">Target position that the enemy moves toward</param>
    /// <param name="speed">How fast the enemy will be moving</param>
    public void SetMovement(Vector2 target, float speed)
    {
        movementManager.SetTargetPosition(target, speed);
    }

    /// <summary>
    /// Returns float representing a damage modifier for enemy attacks
    /// </summary>
    public float GetDamageModifier()
    {
        return statusManager != null ? statusManager.GetStatModifier(EnemyStat.DAMAGE) : 1f;
    }

    /// <summary>
    /// Sets the invincibility status of enemy to isInvincible parameter
    /// </summary>
    public void SetInvincibility(bool isInvincible)
    {
        if (isInvincible)
        {
            statusManager.AddStatusEffect(EnemyStat.INVINCIBILITY, new Status(1f, Mathf.Infinity));
        }
        else
        {
            statusManager.RemoveStatusEffect(EnemyStat.INVINCIBILITY);
        }
    }

    /// <summary>
    /// Returns bool on whether or not enemy has reached its target position
    /// </summary>
    public bool GetIsMoving()
    {
        return movementManager.GetIsMoving();
    }

    /// <summary>
    /// Returns bool on whether or not enemy is facing right
    /// </summary>
    public bool GetIsFacingRight()
    {
        if (GetIsMoving())
        {
            return movementManager.GetIsFacingRight();
        }
        else
        {
            return GetTargetPosition().x - transform.position.x > 0;
        }
    }

    /// <summary>
    /// Used by BehaviorStates to update enemy animation
    /// </summary>
    /// <param name="triggerName">string name of trigger used to set animation</param>
    public void SetAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    private void Stunned()
    {
        Debug.LogFormat("BehaviorStateManager.Stunned");
        ChangeState(stunnedState);
    }

    private void StunRecovery()
    {
        Debug.LogFormat("BehaviorStateManager.StunRecovery");
        ChangeState(recoveredState);
    }
}

public static class EnemyAnimationTriggers
{
    public const string Idle = "IdleTrigger";
    public const string Death = "DeathTrigger";
    public const string Stunned = "StunnedTrigger";
}


public static class SlimeAnimationTriggers
{
    public const string Chase = "ChaseTrigger";
    public const string Charge = "ChargeTrigger";
    public const string Shoot = "ShootTrigger";
    public const string Jump = "JumpTrigger";
    public const string Smash = "SmashTrigger";
}
