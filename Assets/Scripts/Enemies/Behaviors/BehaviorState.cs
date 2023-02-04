using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorState : ScriptableObject
{
    public string behaviorName;

    public abstract void EnterState(BehaviorStateManager manager);

    public abstract void UpdateState(BehaviorStateManager manager);

    public abstract void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other);

    public abstract void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other);

    public char FindTargetTile(BehaviorStateManager manager, float minDistance, float maxDistance)
    {
        Vector2 targetPosition = manager.GetTargetPosition();

        foreach (var tile in StageLayout.Instance.TilePositions)
        {
            float distance = EnemyMovementManager.CalculateDistance(tile.Key, targetPosition);

            if (distance >= minDistance && distance <= maxDistance)
                return tile.Key;
        }

        return ' ';
    }
}
