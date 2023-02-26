using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTail : BasicProjectile
{
    [SerializeField] private float smashSizeMultiplier;

    [SerializeField] private float delayForSpawn;
    
    [SerializeField] private ParticleSystem onSpawnParticles;

    [SerializeField] private float animationMoveSpeed;

    [SerializeField] private float animationYOffset;

    private BehaviorStateManager behaviorManager;
    private EnemyStatusManager statusManager;
    private Animator animator;
    private float timeSinceEnabled;
    private bool hasSpawned;

    private IEnumerator windUpCoroutine;
    private IEnumerator windDownCoroutine;

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
        if (timeSinceEnabled >= delayForSpawn)
        {
            if (!hasSpawned) Spawn();

            currentAirTime += Time.deltaTime;

            if (currentAirTime >= maxAirTime && windDownCoroutine == null)
            {
                windDownCoroutine = WindDown();
                StartCoroutine(windDownCoroutine);
            }
        }
        else
            timeSinceEnabled += Time.deltaTime;
    }

    void OnEnable()
    {
        timeSinceEnabled = 0;

        currentAirTime = 0;

        animator.SetBool("isAttacking", false);
        sprite.color = new Color(0, 0, 0, .5f);
        projectileCollider.enabled = false;

        windUpCoroutine = null;
        windDownCoroutine = null;

        hasSpawned = false;
    }

    private void Spawn()
    {
        animator.SetBool("isAttacking", true);
        sprite.color = new Color(1, 1, 1, 1);
        onSpawnParticles.Play();
        projectileCollider.enabled = true;

        Collider2D[] colliders = Physics2D.OverlapAreaAll(
            new Vector2(transform.position.x - (projectileCollider.bounds.extents.x * smashSizeMultiplier), transform.position.y - (projectileCollider.bounds.extents.y * smashSizeMultiplier)),
            new Vector2(transform.position.x + (projectileCollider.bounds.extents.x * smashSizeMultiplier), transform.position.y + (projectileCollider.bounds.extents.y * smashSizeMultiplier)),
            collisionLayers);

        foreach (Collider2D c in colliders)
        {
            Player hitPlayer = null;
            if (c.TryGetComponent<Player>(out hitPlayer))
            {
                //Damage the player
                hitPlayer.Damage(damage, true, false);
            }
        }

        hasSpawned = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
        hasSpawned = true;
        
        StopAllCoroutines();

        windDownCoroutine = WindDown();
        StartCoroutine(windDownCoroutine);
    }

    public void StartWindUp()
    {
        windUpCoroutine = WindUp();
        StartCoroutine(windUpCoroutine);
    }

    private IEnumerator WindUp()
    {
        Vector3 targetPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y + animationYOffset, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationMoveSpeed * Time.deltaTime);
            timeSinceEnabled = 0f;
            yield return null;
        }
    }

    private IEnumerator WindDown()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + animationYOffset, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationMoveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
