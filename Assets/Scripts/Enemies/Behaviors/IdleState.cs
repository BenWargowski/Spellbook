using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New IdleState", menuName = "Behavior/IdleState")]
public class IdleState : BehaviorState
{
    [SerializeField] private char keyTile;

    public override void EnterState(BehaviorStateManager manager)
    {
        if (StageLayout.Instance.TilePositions.ContainsKey(keyTile))
            manager.SetMovement(StageLayout.Instance.TilePositions[keyTile], manager.DefaultSpeed);
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (!manager.GetIsMoving())
            manager.SetAnimation(EnemyAnimationTriggers.Idle);
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }
}
