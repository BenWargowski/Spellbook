using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChannelHealState", menuName = "Behavior/ChannelHealState")]
public class ChannelHealState : BehaviorState
{
    [SerializeField] private char keyTile;

    [SerializeField] private float healAmount;

    [SerializeField] private float delayBetweenHeal;

    private EnemyHealth health;
    private float timeSinceHealed;

    public override void EnterState(BehaviorStateManager manager)
    {
        health = manager.GetComponent<EnemyHealth>();

        timeSinceHealed = 0;
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (health != null && timeSinceHealed >= delayBetweenHeal)
        {
            health.Heal(healAmount);

            timeSinceHealed = 0;
        }
        else
        {
            timeSinceHealed += Time.deltaTime;
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerStay(BehaviorStateManager manager, Collider2D other)
    {

    }
}
