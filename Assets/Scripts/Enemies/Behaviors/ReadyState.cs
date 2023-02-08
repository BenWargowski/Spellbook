using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Central state that changes BehaviorStateManager's current state when entered to some random state
/// </summary>
[CreateAssetMenu(fileName = "New ReadyState", menuName = "Behavior/ReadyState")]
public class ReadyState : BehaviorState
{
    [SerializeField] private List<BehaviorState> Behaviors;

    [System.NonSerialized] private List<BehaviorState> satchel = new List<BehaviorState>();

    [SerializeField] private float delayBetweenAttacks;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    private float timeSinceReadied;

    public override void EnterState(BehaviorStateManager manager)
    {
        timeSinceReadied = 0;

        manager.SetMovement(StageLayout.Instance.TilePositions[FindTargetTile(manager, manager.transform.position, minDistanceThreshold, maxDistanceThreshold)], manager.DefaultSpeed);

        if (satchel.Count == 0)
        {
            satchel = new List<BehaviorState>(Behaviors);
        }
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceReadied += Time.deltaTime;

        if (timeSinceReadied >= delayBetweenAttacks && !manager.GetIsMoving())
        {
            int j = Random.Range(0, satchel.Count);
            BehaviorState state = satchel[j];
            satchel.RemoveAt(j);

            manager.ChangeState(state);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerStay(BehaviorStateManager manager, Collider2D other)
    {

    }
}
