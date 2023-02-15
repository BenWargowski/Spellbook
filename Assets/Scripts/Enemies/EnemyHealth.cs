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
        set
        {
            if (statusManager.GetStatCount(EnemyStat.INVINCIBILITY) >= 1) return;

            //set value w/ respect to bounds
            currentHealth = Mathf.Clamp(value, 0, this.maxHealth);

            //update health bar
            if (healthBar != null) healthBar.UpdateBar(currentHealth, this.maxHealth);

            if (currentHealth <= 0)
            {
                Death();
            }
        }
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
