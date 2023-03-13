using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template for derived BehaviorStates
/// </summary>
public abstract class BehaviorState : ScriptableObject
{
    public string behaviorName;

    [SerializeField] protected LayerMask onContactCollisionLayers;

    public const float onContactTickDamage = .05f;

    /// <summary>
    /// Called by BehaviorStateManager when entering the BehaviorState
    /// </summary>
    public abstract void EnterState(BehaviorStateManager manager);

    /// <summary>
    /// Called by BehaviorStateManager when exiting the BehaviorState
    /// </summary>
    public abstract void ExitState(BehaviorStateManager manager);

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
    ///  Called by BehaviorStateManager each frame some collider stays in enemy
    /// </summary>
    public virtual void OnStateTriggerStay(BehaviorStateManager manager, Collider2D other)
    {
        if (onContactCollisionLayers == (onContactCollisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Damage(onContactTickDamage * manager.GetDamageModifier(), false, false); //NOTE: does not trigger i-frames
            }
        }
    }

    /// <summary>
    /// Can be used to determine whether or not to enter the state
    /// </summary>
    /// <returns>Returns bool on whether or not to enter the state</returns>
    public virtual bool EnterCondition(BehaviorStateManager manager)
    {
        return true;
    }

    /// <summary>
    /// Finds a keyTile that satisfies the various requirements
    /// </summary>
    /// <param name="target">Target position that's compared against each keyTile to find a suitable keyTile</param>
    /// <param name="minDistance">Minimum distance a keyTile must be from enemy's target</param>
    /// <param name="maxDistance">Maximum distance a keyTile can be from enemy's target</param>
    /// <returns></returns>
    protected char FindTargetTile(BehaviorStateManager manager, Vector2 target, float minDistance, float maxDistance)
    {
        foreach (var tile in StageLayout.Instance.TilePositions)
        {
            float distance = EnemyMovementManager.CalculateDistance(tile.Key, target);

            if (distance >= minDistance && distance <= maxDistance)
                return tile.Key;
        }

        return FindTargetTile(manager, target, minDistance * .95f, maxDistance * 1.05f);
    }
}
