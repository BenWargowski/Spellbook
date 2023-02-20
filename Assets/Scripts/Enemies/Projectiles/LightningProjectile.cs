using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningProjectile : BasicProjectile
{
    [SerializeField] private ParticleSystem onSpawnParticles;

    [SerializeField] private float delayOnSpawn;

    private float timeSinceSpawned;
    private bool hasSpawned;

    void Update()
    {
        if (timeSinceSpawned >= delayOnSpawn)
        {
            if (!hasSpawned) StartCoroutine(Spawn());

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

        sprite.enabled = false;
        projectileCollider.enabled = false;
        trailParticles.Play();

        hasHit = false;
        hasSpawned = false;
    }

    private IEnumerator Spawn()
    {
        sprite.enabled = true;
        onSpawnParticles.Play();

        hasSpawned = true;

        yield return new WaitForSeconds(.15f);

        projectileCollider.enabled = true;
        trailParticles.Stop();
    }
}
