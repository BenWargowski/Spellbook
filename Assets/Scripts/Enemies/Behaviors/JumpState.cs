using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New JumpState", menuName = "Behavior/JumpState")]
public class JumpState : BehaviorState
{
    [SerializeField] private BehaviorState nextState;

    [SerializeField] private float windUp;

    [SerializeField] private float jumpHeight;

    [SerializeField] private float jumpSpeed;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    private float timeElapsed;

    private bool hasJumped;

    public override void EnterState(BehaviorStateManager manager)
    {
        timeElapsed = 0;

        manager.SetAnimation(EnemyAnimationTriggers.WindUp);

        hasJumped = false;
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeElapsed += Time.deltaTime;

        if (hasJumped)
        {
            if (!manager.GetIsMoving())
                manager.ChangeState(nextState);
        }
        else if (timeElapsed >= windUp)
        {
            Jump(manager);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Jump(BehaviorStateManager manager)
    {
        manager.SetMovement(new Vector2(manager.transform.position.x, manager.transform.position.y + jumpHeight), jumpSpeed);
        manager.SetInvincibility(true);

        hasJumped = true;
    }
}
