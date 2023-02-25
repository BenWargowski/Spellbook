using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTail : BasicProjectile
{
    [SerializeField] private float delayForSpawn;
    [SerializeField] private ParticleSystem onSpawnParticles;

    private BehaviorStateManager behaviorManager;
    private EnemyStatusManager statusManager;
    private Animator animator;
    private float timeSinceSpawned;
    private bool hasSpawned;

    protected override void Awake()
    {
        base.Awake();

        animator = GetComponent<Animator>();

        behaviorManager = FindObjectOfType<BehaviorStateManager>();
        if (behaviorManager != null)
            transform.SetParent(behaviorManager.transform);

        statusManager = FindObjectOfType<EnemyStatusManager>();
        if (statusManager != null)
        {
            statusManager.onStunned += Stunned;
        }
    }

    void Update()
    {
        if (timeSinceSpawned >= delayForSpawn)
        {
            if (!hasSpawned) Spawn();

            currentAirTime += Time.deltaTime;

            if (currentAirTime >= maxAirTime)
                gameObject.SetActive(false);
        }
        else
            timeSinceSpawned += Time.deltaTime;
    }

    void OnEnable()
    {
        timeSinceSpawned = 0;

        currentAirTime = 0;

        animator.SetBool("isAttacking", false);
        sprite.color = new Color(1, 1, 1, .85f);
        projectileCollider.enabled = false;

        hasHit = false;
        hasSpawned = false;
    }

    private void Spawn()
    {
        animator.SetBool("isAttacking", true);
        sprite.color = new Color(1, 1, 1, 1);
        onSpawnParticles.Play();
        projectileCollider.enabled = true;
        hasSpawned = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Damage(damage, true, false);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Damage(BehaviorState.onContactTickDamage * behaviorManager.GetDamageModifier(), false, false);
            }
        }
    }

    private void Stunned()
    {
        gameObject.SetActive(false);
    }
}
