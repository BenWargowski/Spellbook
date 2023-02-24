using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : MonoBehaviour
{
    protected BehaviorStateManager manager;
    private EnemyHealth health;
    private SpriteRenderer rend;
    protected bool isActive = true;

    private Material defaultMaterial;
    [SerializeField] private Material flashingMaterial;

    private IEnumerator flashingEffect;
    private float flashingDuration = 1;
    private float flashingDelay = .075f;

    void Awake()
    {
        manager = GetComponentInParent<BehaviorStateManager>();
        health = GetComponentInParent<EnemyHealth>();
        rend = GetComponent<SpriteRenderer>();

        defaultMaterial = rend.material;
    }

    void Start()
    {
        GameEvents.Instance.playerVictory += EnemyDeath;
        health.onDamaged += DamagedVisual;
    }

    void LateUpdate()
    {
        if (!isActive) return;

        if (manager.GetIsFacingRight())
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }
    }

    private void EnemyDeath()
    {
        isActive = false;

        health.onDamaged -= DamagedVisual;
    }

    private void DamagedVisual()
    {
        if (flashingEffect != null)
            StopCoroutine(flashingEffect);

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
