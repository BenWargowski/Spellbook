using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorState : ScriptableObject
{
    public string behaviorName;

    public abstract void EnterState(BehaviorStateManager manager);

    public abstract void UpdateState(BehaviorStateManager manager);

    public abstract void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other);

    public abstract void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other);
}
