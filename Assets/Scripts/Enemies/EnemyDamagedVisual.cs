using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamagedVisual : MonoBehaviour
{
    private EnemyHealth health;
    protected SpriteRenderer rend;

    private Material defaultMaterial;
    [SerializeField] private Material flashingMaterial;

    private IEnumerator flashingEffect;
    private float flashingDuration = .5f;
    private float flashingDelay = .075f;

    protected virtual void Start()
    {
        health = GetComponentInParent<EnemyHealth>();
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;

        health.onDamaged += DamagedVisual;
    }

    private void DamagedVisual()
    {
        if (flashingEffect != null)
            StopCoroutine(flashingEffect);

        if (!gameObject.activeSelf) return;

        flashingEffect = FlashingEffect();
        StartCoroutine(flashingEffect);
    }

    private IEnumerator FlashingEffect()
    {
        float totalTimeElapsed = 0;
        float timeSinceSwapped = 0;

        while (totalTimeElapsed <= flashingDuration)
        {
            totalTimeElapsed += Time.deltaTime;

            if (timeSinceSwapped >= flashingDelay)
            {
                rend.material = SwapMaterial(rend.material);

                timeSinceSwapped = 0;
            }
            else
            {
                timeSinceSwapped += Time.deltaTime;
            }

            yield return null;
        }

        rend.material = defaultMaterial;
    }

    private Material SwapMaterial(Material currentMaterial)
    {
        return currentMaterial == defaultMaterial ? flashingMaterial : defaultMaterial;
    }
}
