using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChaseState", menuName = "Behavior/ChaseState")]
public class ChaseState : BehaviorState
{
    [SerializeField] private List<BehaviorState> nextStates;

    [SerializeField] private float damage;

    [SerializeField] private float maxChasingTime;

    [SerializeField] private float chaseSpeed;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    private char tileKey;
    private float timeSinceChasing;
    private bool isReseting;

    public override void EnterState(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Chase);

        isReseting = false;

        timeSinceChasing = 0;

        Chase(manager);
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceChasing += Time.deltaTime;

        float tileDistanceFromTarget = EnemyMovementManager.CalculateDistance(tileKey, manager.GetTargetPosition());

        if (timeSinceChasing >= maxChasingTime)
        {
            if (!isReseting)
            {
                manager.SetAnimation(EnemyAnimationTriggers.Idle); // TO DO: should change to a walk animation if one is created
                if (manager.DefaultSpeed > 0)
                    manager.SetMovement(StageLayout.Instance.TilePositions[FindTargetTile(manager, manager.transform.position, minDistanceThreshold, maxDistanceThreshold)], manager.DefaultSpeed);
                isReseting = true;
            }
            else if (!manager.GetIsMoving())
                manager.ChangeState(nextStates[Random.Range(0,nextStates.Count)]);
        }
        else if (tileDistanceFromTarget < minDistanceThreshold || tileDistanceFromTarget > maxDistanceThreshold)
        {
            Chase(manager);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {
        if (onContactCollisionLayers == (onContactCollisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Damage(damage * manager.GetDamageModifier(), false, false); //NOTE: does not trigger i-frames
            }
        }
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Chase(BehaviorStateManager manager)
    {
        tileKey = FindTargetTile(manager, manager.GetTargetPosition(), minDistanceThreshold, maxDistanceThreshold);

        manager.SetMovement(StageLayout.Instance.TilePositions[tileKey], chaseSpeed);
    }
}
