using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorStateManager : MonoBehaviour
{
    [SerializeField] private BehaviorState currentState;

    [SerializeField] private List<BehaviorState> behaviors = new List<BehaviorState>();

    [SerializeField] private Transform target; // may change to some player script later

    [field: SerializeField] public float DefaultSpeed { get; private set; }

    private EnemyMovementManager movement;

    void Awake()
    {
        target = GameObject.FindWithTag("Player").transform; // may change with more clean code later

        movement = GetComponent<EnemyMovementManager>();
    }

    void Start()
    {
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
        movement.ResetTargetPosition();

        currentState = newState;
        currentState.EnterState(this);
    }

    public List<BehaviorState> GetBehaviors()
    {
        return new List<BehaviorState>(behaviors);
    }

    public Vector2 GetTargetPosition()
    {
        return target != null ? new Vector2(target.position.x, target.position.y) : Vector2.zero;
    }

    public void SetMovement(char tileKey, float speed)
    {
        movement.SetTargetPosition(tileKey, speed);
    }

    public bool GetIsMoving()
    {
        return movement.HasReachedTarget();
    }
}
