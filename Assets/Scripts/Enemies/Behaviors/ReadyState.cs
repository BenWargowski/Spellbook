using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New ReadyState", menuName = "Behavior/ReadyState")]
public class ReadyState : BehaviorState
{
    [SerializeField] private List<BehaviorState> behaviorsToKeep;

    [System.NonSerialized] private List<BehaviorState> satchel = new List<BehaviorState>();

    public override void EnterState(BehaviorStateManager manager)
    {
        if (satchel.Count == 0)
        {
            List<BehaviorState> temp = manager.GetBehaviors();
            
            for (int i = 0; i < temp.Count; i++)
            {
                if (behaviorsToKeep.Contains(temp[i]))
                {
                    satchel.Add(temp[i]);
                }
            }
        }

        int j = Random.Range(0, satchel.Count);
        BehaviorState state = satchel[j];
        satchel.RemoveAt(j);

        manager.ChangeState(state);
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
