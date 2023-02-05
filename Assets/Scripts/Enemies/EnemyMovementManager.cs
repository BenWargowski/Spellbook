using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * EnemyMovementManager
 * Manages enemy movement by moving them toward target position
 */
public class EnemyMovementManager : MonoBehaviour
{
    private float moveSpeed;

    private bool isMoving;
    
    private Vector2 targetPosition;

    void Awake()
    {
        isMoving = false;
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
    /// Returns bool on whether or not enemy has reached target position
    /// </summary>
    public bool HasReachedTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPosition) <= .01;
    }

    /// <summary>
    /// Updates target position on some c tileKey and begins enemy's movement toward it
    /// </summary>
    public void SetTargetPosition(char c, float newSpeed)
    {
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        targetPosition = StageLayout.Instance.TilePositions[c];
        
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

        float step = moveSpeed * Time.deltaTime;
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
}
