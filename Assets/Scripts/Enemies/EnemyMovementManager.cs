using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    private float moveSpeed;

    private Transform enemy;

    private bool isMoving;
    
    private Vector2 targetPosition;

    void Awake()
    {
        enemy = gameObject.transform;

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
        return Vector2.Distance(new Vector2(enemy.position.x, enemy.position.y), targetPosition) <= .01;
    }

    public void SetTargetPosition(char c, float newSpeed)
    {
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        targetPosition = StageLayout.Instance.TilePositions[c];
        
        moveSpeed = newSpeed;
        
        isMoving = true;
    }

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
