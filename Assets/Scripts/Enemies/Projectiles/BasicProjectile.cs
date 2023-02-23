using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] protected float maxAirTime;

    [SerializeField] protected LayerMask collisionLayers;

    protected Vector3 moveDirection = new Vector3(0, 0, 0);
    protected float currentAirTime;
    protected float damage;
    private float speed;

    protected SpriteRenderer sprite;
    protected CircleCollider2D projectileCollider;
    [SerializeField] protected ParticleSystem trailParticles;
    [SerializeField] protected ParticleSystem onHitParticles;

    protected bool hasHit = false;

    protected IEnumerator onHitCoroutine;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        projectileCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        currentAirTime += Time.deltaTime;

        if (currentAirTime >= maxAirTime && !hasHit)
        {
            onHitCoroutine = OnHit();
            StartCoroutine(onHitCoroutine);
        }
    }

    void OnEnable()
    {
        currentAirTime = 0;

        sprite.enabled = true;
        projectileCollider.enabled = true;
        trailParticles.Play();

        hasHit = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            //Check if the other object is a player
            Player hitPlayer = null;
            if (other.TryGetComponent<Player>(out hitPlayer)) {
                //Damage the player
                hitPlayer.Damage(damage, true, false);
            }

            onHitCoroutine = OnHit();
            StartCoroutine(onHitCoroutine);
        }
    }

    private IEnumerator OnHit()
    {
        hasHit = true;

        sprite.enabled = false;
        projectileCollider.enabled = false;
        trailParticles.Stop();
        onHitParticles.Play();

        while (onHitParticles.isPlaying)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void SetProjectile(Vector3 direction, float damage, float speed)
    {
        moveDirection = direction;
        this.damage = damage;
        this.speed = speed;
    }
}
