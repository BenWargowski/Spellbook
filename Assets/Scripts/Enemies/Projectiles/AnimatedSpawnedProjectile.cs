using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSpawnedProjectile : BasicProjectile
{
    [SerializeField] private float tickDamageMultiplier;

    [SerializeField] protected float delayForSpawn;

    [SerializeField] protected ParticleSystem onSpawnParticles;

    [SerializeField] protected float animationMoveSpeed;

    [SerializeField] protected float animationYOffset;


    protected Animator animator;
    protected float timeSinceEnabled;
    protected bool hasSpawned;
    protected bool isWindingDown;

    protected IEnumerator windUpCoroutine;
    protected IEnumerator windDownCoroutine;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();

        base.Awake();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceEnabled >= delayForSpawn)
        {
            if (!hasSpawned) Spawn();

            currentAirTime += Time.deltaTime;

            if (currentAirTime >= maxAirTime && !isWindingDown)
            {
                windDownCoroutine = WindDown();
                StartCoroutine(windDownCoroutine);
            }
        }
        else
            timeSinceEnabled += Time.deltaTime;
    }

    protected virtual void OnEnable()
    {
        timeSinceEnabled = 0;

        currentAirTime = 0;

        projectileCollider.enabled = false;

        if (trailParticles != null)
            trailParticles.Play();

        windUpCoroutine = null;
        windDownCoroutine = null;

        isWindingDown = false;
        hasSpawned = false;
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
                hitPlayer.Damage(damage * tickDamageMultiplier, false, false);
            }
        }
    }

    public override void SetProjectile(Vector3 direction, float damage, float speed)
    {
        windUpCoroutine = WindUp();
        StartCoroutine(windUpCoroutine);

        base.SetProjectile(direction, damage, speed);
    }

    protected virtual void Spawn()
    {
        projectileCollider.enabled = true;

        onSpawnParticles.Play();

        hasSpawned = true;
    }

    protected virtual IEnumerator WindUp()
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

    protected virtual IEnumerator WindDown()
    {
        isWindingDown = true;

        yield return null;

        projectileCollider.enabled = false;

        gameObject.SetActive(false);
    }
}
