using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ChargeState", menuName = "Behavior/ChargeState")]
public class ChargeState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private float damage;

    [SerializeField] private float chargeSpeed;

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

        chargeParticlesInstance.transform.position = manager.transform.position;
        chargeParticlesInstance.Play();

        timeSinceCharged = 0;

        hasCharged = false;
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        chargeParticlesInstance.transform.position = manager.transform.position;

        if (hasCharged)
        {
            if (manager.GetIsMoving())
            {
                chargeParticlesInstance.Stop();

                manager.ChangeState(returningState);
            }

            return;
        }

        timeSinceCharged += Time.deltaTime;

        if (timeSinceCharged >= windUp)
        {
            tileKey = FindTargetTile(manager, minDistanceThreshold, maxDistanceThreshold);

            Charge(manager);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer)) {
                //Damage the player
                hitPlayer.Health -= this.damage;
            }
        }
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Charge(BehaviorStateManager manager)
    {
        manager.SetMovement(tileKey, chargeSpeed);

        hasCharged = true;
    }
}
