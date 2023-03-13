using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TeleportState", menuName = "Behavior/TeleportState")]
public class TeleportState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private TeleportStyle style;

    [SerializeField] private List<Vector2> teleportLocations;

    [SerializeField] private float windUp;

    [SerializeField] private float windDown;

    [SerializeField] private GameObject teleportParticlesPrefab;

    [SerializeField] private LayerMask checkCollisionLayers;

    [SerializeField] private float checkRadius;

    [SerializeField] private AudioClip clip;

    private Vector2 teleportDestination;
    private float timeSinceEntered;
    private float timeSinceTeleported;
    private bool hasTeleported;
    private ParticleSystem initialParticlesInstance;
    private ParticleSystem destinationParticlesInstance;

    public override void EnterState(BehaviorStateManager manager)
    {
        if (initialParticlesInstance == null)
            initialParticlesInstance = Instantiate(teleportParticlesPrefab, manager.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        if (destinationParticlesInstance == null)
            destinationParticlesInstance = Instantiate(teleportParticlesPrefab, manager.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();

        timeSinceEntered = 0;
        timeSinceTeleported = 0;
        hasTeleported = false;

        teleportDestination =  style == TeleportStyle.ATTACK ? GetAttackTeleportLocation(manager) : GetSafeTeleportLocation(manager);

        initialParticlesInstance.transform.position = manager.transform.position;
        initialParticlesInstance.Play();

        destinationParticlesInstance.transform.position = new Vector3(teleportDestination.x, teleportDestination.y, manager.transform.position.z);
        destinationParticlesInstance.Play();
    }

    public override void ExitState(BehaviorStateManager manager)
    {
        if (initialParticlesInstance.isPlaying) initialParticlesInstance.Stop();
        if (destinationParticlesInstance.isPlaying) destinationParticlesInstance.Stop();
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceEntered += Time.deltaTime;

        if (timeSinceEntered < windUp) return;

        if (!hasTeleported)
            Teleport(manager);
        else
        {
            timeSinceTeleported += Time.deltaTime;

            if (timeSinceTeleported >= windDown)
                manager.ChangeState(returningState);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override bool EnterCondition(BehaviorStateManager manager)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(manager.transform.position, checkRadius, checkCollisionLayers);

        return style == TeleportStyle.ATTACK ? colliders.Length == 0 && teleportLocations.Count > 0 : colliders.Length > 0 && teleportLocations.Count > 0;
    }

    private void Teleport(BehaviorStateManager manager)
    {
        initialParticlesInstance.Stop();
        destinationParticlesInstance.Stop();
        SoundManager.Instance.PlaySound(clip);

        manager.transform.position = new Vector3(teleportDestination.x, teleportDestination.y, manager.transform.position.z);

        hasTeleported = true;
    }

    private Vector2 GetAttackTeleportLocation(BehaviorStateManager manager)
    {
        Vector2 newTeleportLocation = new Vector2(manager.transform.position.x, manager.transform.position.y);
        float minDistance = Mathf.Infinity;

        for (int i = 0; i < teleportLocations.Count; i++)
        {
            float newDistance = Vector2.Distance(manager.GetTargetPosition(), teleportLocations[i]);

            if (newDistance < minDistance && manager.transform.position != new Vector3(teleportLocations[i].x, teleportLocations[i].y, manager.transform.position.z))
            {
                newTeleportLocation = teleportLocations[i];
                minDistance = newDistance;
            }
        }

        return newTeleportLocation;
    }

    private Vector2 GetSafeTeleportLocation(BehaviorStateManager manager)
    {
        Vector2 newTeleportLocation = new Vector2(manager.transform.position.x, manager.transform.position.y);
        float lowestDangerLevel = Mathf.Infinity;

        for (int i = 0; i < teleportLocations.Count; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(teleportLocations[i].x, teleportLocations[i].y, manager.transform.position.z), checkRadius, checkCollisionLayers);

            if (colliders.Length < lowestDangerLevel)
            {
                newTeleportLocation = teleportLocations[i];
                lowestDangerLevel = colliders.Length;
            }
        }

        return newTeleportLocation;
    }
}

public enum TeleportStyle
{
    ATTACK,
    DEFENSE
}
