using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy movement by moving them toward target position
/// </summary>
public class EnemyMovementManager : MonoBehaviour
{
    [SerializeField] private char startingKey;

    private EnemyStatusManager statusManager;

    private float moveSpeed;

    private bool isMoving;
    
    private Vector2 targetPosition;

    void Awake()
    {
        isMoving = false;

        statusManager = GetComponent<EnemyStatusManager>();
        statusManager.onStunned += Stunned;
        statusManager.onNotStunned += StunRecovery;
    }

    void Start()
    {
        transform.position = StageLayout.Instance.TilePositions[startingKey];
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// Returns the float distance between the c tileKey's position and target position
    /// </summary>
    public static float CalculateDistance(char c, Vector2 target)
    {
        return Vector2.Distance(StageLayout.Instance.TilePositions[c], new Vector2(target.x, target.y));
    }

    /// <summary>
    /// Returns bool on whether or not enemy is moving
    /// </summary>
    /// <returns></returns>
    public bool GetIsMoving()
    {
        return isMoving;
    }

    /// <summary>
    /// Returns bool on whether or not enmey is facing right when moving
    /// </summary>
    /// <returns></returns>
    public bool GetIsFacingRight()
    {
        return targetPosition.x - transform.position.x > 0;
    }

    /// <summary>
    /// Updates target position to some Vector2 and begins enemy's movement toward it
    /// </summary>
    public void SetTargetPosition(Vector2 target, float newSpeed)
    {
        targetPosition = target;
        
        moveSpeed = newSpeed;
        
        isMoving = true;
    }

    /// <summary>
    /// Halts enemy movement
    /// </summary>
    public void ResetTargetPosition()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.y);

        isMoving = false;
    }

    /// <summary>
    /// Handles movement mechanic
    /// </summary>
    private void Move()
    {
        if (!isMoving) return;

        float step = moveSpeed * Time.deltaTime * (statusManager != null ? statusManager.GetSpeedMod() : 1);
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            step
        );

        if (Vector2.Distance(transform.position, targetPosition) <= 0.01)
        {
            isMoving = false;
        }
    }

    private void Stunned()
    {
        Debug.LogFormat("EnemyMovementManager.Stunned");
        ResetTargetPosition();
    }

    private void StunRecovery()
    {
        Debug.LogFormat("EnemyMovementManager.StunRecovery");
    }
}
