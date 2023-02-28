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
    [SerializeField] protected List<BehaviorState> Behaviors;

    [System.NonSerialized] protected List<BehaviorState> satchel = new List<BehaviorState>();

    [SerializeField] protected float delayBetweenAttacks;

    protected float timeSinceReadied;

    public override void EnterState(BehaviorStateManager manager)
    {
        timeSinceReadied = 0;

        manager.SetAnimation(EnemyAnimationTriggers.Idle);

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

        if (!manager.GetIsMoving() && timeSinceReadied >= delayBetweenAttacks)
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
}
