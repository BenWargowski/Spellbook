using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * BehaviorState
 * Template for derived BehaviorStates
 */
public abstract class BehaviorState : ScriptableObject
{
    public string behaviorName;

    /// <summary>
    /// Called by BehaviorStateManager when entering the BehaviorState
    /// </summary>
    public abstract void EnterState(BehaviorStateManager manager);

    /// <summary>
    /// Called by BehaviorStateManager every Update
    /// </summary>
    public abstract void UpdateState(BehaviorStateManager manager);

    /// <summary>
    /// Called by BehaviorStateManager when some collider enters enemy
    /// </summary>
    public abstract void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other);

    /// <summary>
    /// Called by BehaviorStateManager when some collider exits enemy
    /// </summary>
    public abstract void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other);

    /// <summary>
    /// Finds a keyTile that satisfies the distance requirements
    /// </summary>
    /// <param name="minDistance">Minimum distance a keyTile must be from enemy's target</param>
    /// <param name="maxDistance">Maximum distance a keyTile can be from enemy's target</param>
    /// <returns></returns>
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
