using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New StunnedState", menuName = "Behavior/StunnedState")]
public class StunnedState : BehaviorState
{
    public override void EnterState(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Stunned);
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {

    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }
}
