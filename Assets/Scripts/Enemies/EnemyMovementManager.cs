using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static float CalculateDistance(char c, Vector2 target)
    {
        return Vector2.Distance(StageLayout.Instance.TilePositions[c], new Vector2(target.x, target.y));
    }

    public bool HasReachedTarget()
    {
        return Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPosition) <= .01;
    }

    public void SetTargetPosition(char c, float newSpeed)
    {
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        targetPosition = StageLayout.Instance.TilePositions[c];
        
        moveSpeed = newSpeed;
        
        isMoving = true;
    }

    public void ResetTargetPosition()
    {
        targetPosition = new Vector2(transform.position.x, transform.position.y);

        isMoving = false;
    }

    private void Move()
    {
        if (isMoving) Debug.Log("isMoving");

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
