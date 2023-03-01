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

        statusManager = FindObjectOfType<EnemyStatusManager>();
        if (statusManager != null)
        {
            statusManager.onStunned += Stunned;
        }

        base.Awake();
    }

    protected override void OnEnable()
    {
        animator.SetBool("isAttacking", false);
        sprite.color = new Color(0, 0, 0, .5f);

        base.OnEnable();
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

        animator.SetBool("isAttacking", true);
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

    private void Stunned()
    {
        if (!gameObject.activeSelf) return;

        hasSpawned = true;
        
        StopAllCoroutines();

        windDownCoroutine = WindDown();
        StartCoroutine(windDownCoroutine);
    }

    protected override IEnumerator WindDown()
    {
        isWindingDown = true;

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
