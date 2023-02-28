using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDoTStatus : Status
{
    private EnemyHealth enemyHealth;
    private SpellElement element;
    private bool ignoreResistance;

    public EnemyDoTStatus(EnemyHealth health, SpellElement spellElement, bool ignoreResistance, float damage, float duration) : base(damage, duration)
    {
        enemyHealth = health;
        element = spellElement;
        this.ignoreResistance = ignoreResistance;
    }

    public override void UpdateStatus()
    {
        base.UpdateStatus();

        enemyHealth.Damage(modifier * Time.deltaTime, element, ignoreResistance);
    }
}
