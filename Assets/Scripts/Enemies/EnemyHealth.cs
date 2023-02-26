using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy health and death
/// </summary>
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float maxHealth;

    private EnemyStatusManager statusManager;

    private float currentHealth;

    public float Health
    {
        get
        {
            return currentHealth;
        }
        private set
        {
            //set value w/ respect to bounds
            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            //update health bar
            if (healthBar != null) healthBar.UpdateBar(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                Death();
            }
        }
    }

    public event Action onDamaged;
    public void Damage(float damage, SpellElement spellElement, bool ignoreResistance)
    {
        if (statusManager.IsInvincible) return;

        if (!ignoreResistance)
        {
            switch (spellElement)
            {
                case SpellElement.FIRE:
                    damage = (damage * damage) / (damage + statusManager.FireResistance);
                    break;
                case SpellElement.LIGHTNING:
                    damage = (damage * damage) / (damage + statusManager.LightningResistance);
                    break;
                case SpellElement.GROUND:
                    damage = (damage * damage) / (damage + statusManager.RockResistance);
                    break;
            }
        }

        if (damage <= 0) return;

        Health -= damage;

        if (onDamaged != null)
            onDamaged();
    }

    public void Heal(float healAmount)
    {
        if (healAmount <= 0) return;

        Health += healAmount;
    }
    
    void Awake()
    {
        statusManager = GetComponent<EnemyStatusManager>();
    }

    void Start()
    {
        GameEvents.Instance.playerDeath += GainInvincibility;

        Health = maxHealth;
    }

    private void Death()
    {
        GameEvents.Instance.PlayerVictory();
    }

    private void GainInvincibility()
    {
        statusManager.AddStatusEffect(EnemyStat.INVINCIBILITY, new Status(1f, Mathf.Infinity));
    }
}
