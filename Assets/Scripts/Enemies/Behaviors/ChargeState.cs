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

    [SerializeField] private Vector2 particlesPosition;

    private char tileKey;
    private float timeSinceCharged;
    private bool hasCharged;
    private ParticleSystem chargeParticlesInstance;

    public override void EnterState(BehaviorStateManager manager)
    {
        if (chargeParticlesInstance == null)
            chargeParticlesInstance = Instantiate(particlesPrefab, manager.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        chargeParticlesInstance.transform.position = manager.transform.position + new Vector3((manager.GetIsFacingRight() ? 1 : -1) * particlesPosition.x, particlesPosition.y, 0);
        chargeParticlesInstance.Play();

        timeSinceCharged = 0;

        hasCharged = false;
    }

    public override void ExitState(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Idle);

        chargeParticlesInstance.Stop();
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        chargeParticlesInstance.transform.position = manager.transform.position + new Vector3((manager.GetIsFacingRight() ? 1 : -1) * particlesPosition.x, particlesPosition.y, 0);

        timeSinceCharged += Time.deltaTime;

        if (hasCharged)
        {
            if (!manager.GetIsMoving())
                manager.ChangeState(returningState);
        }
        else if (timeSinceCharged >= windUp)
        {
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
                hitPlayer.Damage(damage * manager.GetDamageModifier(), true, false);
            }
        }
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerStay(BehaviorStateManager manager, Collider2D other)
    {

    }

    private void Charge(BehaviorStateManager manager)
    {
        manager.SetAnimation(SlimeAnimationTriggers.Charge);

        tileKey = FindTargetTile(manager, manager.GetTargetPosition(), minDistanceThreshold, maxDistanceThreshold);

        manager.SetMovement(StageLayout.Instance.TilePositions[tileKey], chargeSpeed);

        hasCharged = true;
    }
}
