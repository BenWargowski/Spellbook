using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemy's various statuses
/// </summary>
public class EnemyStatusManager : MonoBehaviour
{
    private Dictionary<EnemyStat, HashSet<Status>> statusEffects = new Dictionary<EnemyStat, HashSet<Status>>();

    public event Action onStunned;

    public event Action onNotStunned;

    [SerializeField] private float baseFireResist;
    [SerializeField] private float baseLightningResist;
    [SerializeField] private float baseRockResist;

    public bool IsStunned => (GetStatCount(EnemyStat.STUNNED) >= 1);
    public bool IsInvincible => (GetStatCount(EnemyStat.INVINCIBILITY) >= 1);
    public float FireResistance => (Mathf.Max(0f, baseFireResist + GetStatModifier(EnemyStat.FIRE_RESISTANCE, ModifierType.ADDITIVE)));
    public float LightningResistance => (Mathf.Max(0f, baseLightningResist + GetStatModifier(EnemyStat.LIGHTNING_RESISTANCE, ModifierType.ADDITIVE)));
    public float RockResistance => (Mathf.Max(0f, baseRockResist + GetStatModifier(EnemyStat.ROCK_RESISTANCE, ModifierType.ADDITIVE)));

    void Awake()
    {
        var statusCategories = Enum.GetValues(typeof(EnemyStat));
        foreach (EnemyStat statCategory in statusCategories)
        {
            statusEffects[statCategory] = new HashSet<Status>();
        }
    }

    void Update()
    {
        foreach (KeyValuePair<EnemyStat, HashSet<Status>> pair in statusEffects)
        {
            foreach (Status status in pair.Value)
            {
                status.UpdateStatus();
            }

            int statusesRemovedCount = pair.Value.RemoveWhere((x) => !x.IsValid());

            if (pair.Key == EnemyStat.STUNNED && statusesRemovedCount > 0 && !IsStunned && onNotStunned != null)
                onNotStunned();
        }
    }

    public void AddStatusEffect(EnemyStat statCategory, Status status)
    {
        if (IsInvincible && statCategory != EnemyStat.INVINCIBILITY) return;

        statusEffects[statCategory].Add(status);

        if (statCategory == EnemyStat.STUNNED && IsStunned && onStunned != null)
            onStunned();

        if (IsInvincible)
        {
            List<EnemyStat> additiveStats = new List<EnemyStat> { EnemyStat.FIRE_RESISTANCE, EnemyStat.LIGHTNING_RESISTANCE, EnemyStat.ROCK_RESISTANCE, EnemyStat.STUNNED, EnemyStat.OTHER };

            foreach (KeyValuePair<EnemyStat, HashSet<Status>> pair in statusEffects)
            {
                if (additiveStats.Contains(pair.Key))
                    pair.Value.RemoveWhere((x) => x.modifier <= 0);
                else
                    pair.Value.RemoveWhere((x) => x.modifier < 1);
            }
        }
    }

    public void RemoveStatusEffect(EnemyStat statCategory)
    {
        if (GetStatCount(statCategory) == 0) return;

        Status statToRemove = null;

        foreach (Status status in statusEffects[statCategory])
        {
            if (statToRemove == null || status.timeRemaining < statToRemove.timeRemaining)
                statToRemove = status;
        }
        
        statusEffects[statCategory].Remove(statToRemove);
    }

    public float GetStatModifier(EnemyStat enemyStat, ModifierType type = ModifierType.MULTIPLICATIVE)
    {
        float mod = (type == ModifierType.ADDITIVE ? 0 : 1);

        foreach (Status status in statusEffects[enemyStat])
        {
            mod += status.modifier;
        }

        return (type == ModifierType.ADDITIVE ? mod : Mathf.Max(0, mod));
    }

    public int GetStatCount(EnemyStat enemyStat)
    {
        return statusEffects[enemyStat].Count;
    }
}

public enum EnemyStat
{
    DAMAGE,
    MOVEMENT_SPEED,
    STUNNED,
    INVINCIBILITY,
    FIRE_RESISTANCE,
    LIGHTNING_RESISTANCE,
    ROCK_RESISTANCE,
    OTHER
}

public enum ModifierType
{
    ADDITIVE,
    MULTIPLICATIVE
}
