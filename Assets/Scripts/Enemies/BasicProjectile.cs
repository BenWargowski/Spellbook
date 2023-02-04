using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection = new Vector3(1, 0, 0);

    [SerializeField] float speed;

    [SerializeField] float maxAirTime;

    [SerializeField] float damage;

    [SerializeField] private LayerMask collisionLayers;

    private float currentAirTime;

    private SpriteRenderer sprite;
    private CircleCollider2D projectileCollider;
    [SerializeField] private ParticleSystem trailParticles;
    [SerializeField] private ParticleSystem onHitParticles;

    private bool hasHit = false;

    private IEnumerator onHitCoroutine;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        projectileCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);

        currentAirTime += Time.deltaTime;

        if (currentAirTime >= maxAirTime)
        {
            gameObject.SetActive(false);
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
        else
            hasHit = true;

        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
        {
            // other.gameObject.GetComponent (need player stat script)
        }

        onHitCoroutine = OnHit();
        StartCoroutine(onHitCoroutine);
    }

    private IEnumerator OnHit()
    {
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

    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
    }
}
