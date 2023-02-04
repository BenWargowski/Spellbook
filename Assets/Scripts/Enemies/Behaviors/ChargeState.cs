using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChargeState", menuName = "Behavior/ChargeState")]
public class ChargeState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private float damage;

    [SerializeField] private float windUp;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    [SerializeField] private LayerMask collisionLayers;

    [SerializeField] private GameObject particlesPrefab;

    private char tileKey;
    private float timeSinceCharged;
    private bool hasCharged;
    private ParticleSystem chargeParticlesInstance;

    public override void EnterState(BehaviorStateManager manager)
    {
        if (chargeParticlesInstance == null)
            chargeParticlesInstance = Instantiate(particlesPrefab, manager.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        tileKey = FindTargetTile(manager, minDistanceThreshold, maxDistanceThreshold);

        chargeParticlesInstance.Play();

        timeSinceCharged = 0;

        hasCharged = false;
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        chargeParticlesInstance.transform.position = manager.transform.position;

        if (hasCharged)
        {
            if (manager.movement.HasReachedTarget())
            {

                chargeParticlesInstance.Stop();

                manager.ChangeState(returningState);
            }

            return;
        }

        timeSinceCharged += Time.deltaTime;

        if (timeSinceCharged >= windUp)
        {
            Charge(tileKey, manager);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            // other.gameObject.GetComponent (need player stat script)

            Debug.LogFormat("ChargeState: Hit player");
        }
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Charge(char tileKey, BehaviorStateManager manager)
    {
        manager.movement.SetTargetPosition(tileKey);

        hasCharged = true;
    }
}
