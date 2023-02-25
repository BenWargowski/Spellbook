using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoTStatus : Status
{
    private EnemyHealth enemyHealth;
    private SpellType type;
    private bool ignoreResistance;

    public EnemyDoTStatus(EnemyHealth health, SpellType spellType, bool ignoreResistance, float damage, float duration) : base(damage, duration)
    {
        enemyHealth = health;
        type = spellType;
        this.ignoreResistance = ignoreResistance;
    }

    public override void UpdateStatus()
    {
        base.UpdateStatus();

        enemyHealth.Damage(modifier * Time.deltaTime, type, ignoreResistance);
    }
}
