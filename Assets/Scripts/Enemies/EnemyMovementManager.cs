using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementManager : MonoBehaviour
{
    [SerializeField] private int moveSpeed;

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

    public void SetTargetPosition(char c)
    {
        if (!StageLayout.Instance.TilePositions.ContainsKey(c)) return;

        targetPosition = StageLayout.Instance.TilePositions[c];
        isMoving = true;
    }

    private void Move()
    {
        if (!this.isMoving) return;

        float step = this.moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(
            transform.position,
            this.targetPosition,
            step
        );

        if (Vector2.Distance(transform.position, this.targetPosition) <= 0.01)
        {
            this.isMoving = false;
        }
    }
}
