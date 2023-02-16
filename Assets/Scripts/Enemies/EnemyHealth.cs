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
            if (statusManager.IsInvincible) return;

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

    public void Damage(float damage, SpellType spellType, bool ignoreResistance)
    {
        if (!ignoreResistance)
        {
            switch (spellType)
            {
                case SpellType.FIRE:
                    damage = (damage * damage) / (damage + statusManager.FireResistance);
                    break;
                case SpellType.LIGHTNING:
                    damage = (damage * damage) / (damage + statusManager.LightningResistance);
                    break;
                case SpellType.ROCK:
                    damage = (damage * damage) / (damage + statusManager.RockResistance);
                    break;
            }
        }

        if (damage <= 0) return;

        Health -= damage;
    }

    public void Heal(float healAmount)
    {
        if (healAmount >= 0) return;

        Health += healAmount;
    }
    
    void Awake()
    {
        statusManager = GetComponent<EnemyStatusManager>();
    }

    void Start()
    {
        Health = maxHealth;
    }

    private void Death()
    {
        GameEvents.Instance.PlayerVictory();
    }
}
