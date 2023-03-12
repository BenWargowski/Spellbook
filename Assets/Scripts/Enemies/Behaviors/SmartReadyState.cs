using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central state for enemy that uses BehaviorState.EnterCondition when deciding what state to enter
/// </summary>
[CreateAssetMenu(fileName = "New SmartReadyState", menuName = "Behavior/SmartReadyState")]
public class SmartReadyState : ReadyState
{
    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceReadied += Time.deltaTime;

        if (!manager.GetIsMoving() && timeSinceReadied >= delayBetweenAttacks)
        {
            BehaviorState newState = ChooseState(manager);
            
            if (newState != null) manager.ChangeState(newState);
        }
    }

    /// <summary>
    /// Returns a suitable state to change into
    /// </summary>
    /// <returns>A BehaviorState to change into</returns>
    private BehaviorState ChooseState(BehaviorStateManager manager)
    {
        BehaviorState chosenState = null;

        if (satchel.Count == 0)
        {
            satchel = new List<BehaviorState>(Behaviors);
        }

        int j = Random.Range(0, satchel.Count);
        BehaviorState state = satchel[j];
        satchel.RemoveAt(j);

        if (state.EnterCondition(manager))
            chosenState = state;

        return chosenState;
    }
}
