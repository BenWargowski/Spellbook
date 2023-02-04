using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorStateManager : MonoBehaviour
{
    [SerializeField] private BehaviorState currentState;

    [SerializeField] private List<BehaviorState> behaviors = new List<BehaviorState>();

    [SerializeField] private GameObject target; // may change to some player script later
    
    void Start()
    {
        target = GameObject.FindWithTag("Player"); // may change with more clean code later

        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnStateTriggerEnter(this, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentState.OnStateTriggerExit(this, other);
    }

    public void ChangeState(BehaviorState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public List<BehaviorState> GetBehaviors()
    {
        return new List<BehaviorState>(behaviors);
    }

    public Vector3 GetTargetPosition()
    {
        return target != null ? target.transform.position : Vector3.zero;
    }
}
