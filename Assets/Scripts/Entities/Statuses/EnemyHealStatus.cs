using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealStatus : Status
{
    private EnemyHealth enemyHealth;

    public EnemyHealStatus(EnemyHealth health, float healAmount, float duration) : base(healAmount, duration)
    {
        enemyHealth = health;
    }

    public override void UpdateStatus()
    {
        base.UpdateStatus();

        enemyHealth.Heal(modifier * Time.deltaTime);
    }
}
