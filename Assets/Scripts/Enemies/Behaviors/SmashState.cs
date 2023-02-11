using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SmashState", menuName = "Behavior/SmashState")]
public class SmashState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private float damage;

    [SerializeField] private float smashSpeed;

    [SerializeField] private float smashRadius;

    [SerializeField] private float windDown;

    [SerializeField] private float minDistanceThreshold;

    [SerializeField] private float maxDistanceThreshold;

    [SerializeField] private LayerMask collisionLayers;

    [SerializeField] private GameObject particlesPrefab;

    private char tileKey;
    private bool hasSmashed;
    private float timeSinceSmashed;
    private ParticleSystem smashParticlesInstance;

    public override void EnterState(BehaviorStateManager manager)
    {
        if (smashParticlesInstance == null)
            smashParticlesInstance = Instantiate(particlesPrefab, manager.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        Vector2 target = manager.GetTargetPosition();
        manager.transform.position = new Vector3(target.x, manager.transform.position.y + target.y, manager.transform.position.z);

        timeSinceSmashed = 0;

        hasSmashed = false;

        Move(manager);
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (!manager.GetIsMoving())
        {
            if (!hasSmashed)
            {
                Smash(manager);
            }
            else
            {
                timeSinceSmashed += Time.deltaTime;
            }

            if (timeSinceSmashed >= windDown)
            {
                manager.ChangeState(returningState);
            }
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

    private void Move(BehaviorStateManager manager)
    {
        tileKey = FindTargetTile(manager, manager.GetTargetPosition(), minDistanceThreshold, maxDistanceThreshold);

        manager.SetMovement(StageLayout.Instance.TilePositions[tileKey], smashSpeed);
    }

    private void Smash(BehaviorStateManager manager)
    {
        smashParticlesInstance.transform.position = manager.transform.position;
        smashParticlesInstance.Play();

        Vector2 origin = new Vector2(manager.transform.position.x, manager.transform.position.y);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, smashRadius, collisionLayers);

        foreach (Collider2D c in colliders)
        {
            Player hitPlayer = null;
            if (c.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Health -= damage;
            }
        }

        hasSmashed = true;
    }
}
