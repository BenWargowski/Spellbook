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
    private float flashingDelay = .05f;

    private float totalTimeElapsed;
    private float timeSinceSwapped;
    private bool inProgress;

    protected virtual void Start()
    {
        health = GetComponentInParent<EnemyHealth>();
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;

        health.onDamaged += DamagedVisual;
    }

    private void DamagedVisual()
    {
        if (!gameObject.activeSelf) return;

        if (inProgress)
        {
            totalTimeElapsed = 0;
        }
        else
        {
            flashingEffect = FlashingEffect();
            StartCoroutine(flashingEffect);
        }
    }

    private IEnumerator FlashingEffect()
    {
        inProgress = true;
        totalTimeElapsed = 0;
        timeSinceSwapped = 0;

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
        inProgress = false;
    }

    private Material SwapMaterial(Material currentMaterial)
    {
        return currentMaterial == defaultMaterial ? flashingMaterial : defaultMaterial;
    }
}
