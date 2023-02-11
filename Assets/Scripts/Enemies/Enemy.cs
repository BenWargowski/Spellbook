using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages enemy health and death
/// </summary>
public class Enemy : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float maxHealth;

    private float currentHealth;

    public float Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
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

    void Start()
    {
        Health = maxHealth;
    }

    private void Death()
    {
        GameEvents.Instance.PlayerVictory();
    }
}
