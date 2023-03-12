using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonTail : AnimatedSpawnedProjectile
{
    [SerializeField] private float smashSizeMultiplier;
    
    private BehaviorStateManager behaviorManager;
    private EnemyStatusManager statusManager;

    protected override void Awake()
    {
        behaviorManager = FindObjectOfType<BehaviorStateManager>();
        if (behaviorManager != null)
            transform.SetParent(behaviorManager.transform);

        statusManager = GetComponentInParent<EnemyStatusManager>();
        if (statusManager != null)
        {
            statusManager.onStunned += CancelAttack;
        }

        base.Awake();
    }

    void Start()
    {
        GameEvents.Instance.playerVictory += CancelAttack;
    }

    protected override void OnEnable()
    {
        animator.SetTrigger("Shadow");
        sprite.color = new Color(0, 0, 0, .5f);

        timeSinceEnabled = 0;

        currentAirTime = 0;

        sprite.enabled = true;
        sprite.flipX = transform.position.x < behaviorManager.transform.position.x;
        projectileCollider.enabled = false;

        windUpCoroutine = null;
        windDownCoroutine = null;

        isWindingUp = true;
        isWindingDown = false;
        hasSpawned = false;
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

    protected override void Spawn()
    {
        base.Spawn();

        animator.SetTrigger("Normal");
        sprite.color = new Color(1, 1, 1, 1);
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
    }

    private void CancelAttack()
    {
        if (!gameObject.activeSelf) return;

        hasSpawned = true;

        StopCoroutine(windUpCoroutine);
        
        if (!isWindingDown)
        {
            windDownCoroutine = WindDown();
            StartCoroutine(windDownCoroutine);
        }
    }

    protected override IEnumerator WindDown()
    {
        isWindingDown = true;
        animator.SetTrigger("Normal");

        projectileCollider.enabled = false;

        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + animationYOffset, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > .01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationMoveSpeed * Time.deltaTime);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
