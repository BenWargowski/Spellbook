using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AimState", menuName = "Behavior/AimState")]
public class AimState : BehaviorState
{
    [SerializeField] private BehaviorState nextState;
    
    public override void EnterState(BehaviorStateManager manager)
    {

    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (manager.GetIsFacingTarget())
            manager.ChangeState(nextState);
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }
}
