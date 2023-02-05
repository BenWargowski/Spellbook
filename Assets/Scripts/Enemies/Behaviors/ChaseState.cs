using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChaseState", menuName = "Behavior/ChaseState")]
public class ChaseState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private float damage;

    [SerializeField] private float maxChasingTime;

    [SerializeField] private float chaseSpeed;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    [SerializeField] private LayerMask collisionLayers;

    private char tileKey;
    private float timeSinceChasing;

    public override void EnterState(BehaviorStateManager manager)
    {
        timeSinceChasing = 0;

        Chase(manager);
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceChasing += Time.deltaTime;

        if (timeSinceChasing >= maxChasingTime)
        {
            manager.ChangeState(returningState);
        }

        float tileDistanceFromTarget = EnemyMovementManager.CalculateDistance(tileKey, manager.GetTargetPosition());

        if (tileDistanceFromTarget < minDistanceThreshold || tileDistanceFromTarget > maxDistanceThreshold)
        {
            Chase(manager);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            // other.gameObject.GetComponent (need player stat script)
        }
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Chase(BehaviorStateManager manager)
    {
        tileKey = FindTargetTile(manager, minDistanceThreshold, maxDistanceThreshold);

        manager.SetMovement(tileKey, chaseSpeed);
    }
}
