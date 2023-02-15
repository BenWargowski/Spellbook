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

            if (pair.Key == EnemyStat.STUNNED && statusesRemovedCount > 0 && GetStatCount(EnemyStat.STUNNED) == 0 && onNotStunned != null)
                onNotStunned();
        }
    }

    public void AddStatusEffect(EnemyStat statCategory, Status status)
    {
        if (GetStatCount(EnemyStat.INVINCIBILITY) >= 1 && statCategory != EnemyStat.INVINCIBILITY) return;

        statusEffects[statCategory].Add(status);

        if (statCategory == EnemyStat.STUNNED && GetStatCount(EnemyStat.STUNNED) == 1 && onStunned != null)
            onStunned();
    }

    public void RemoveStatusEffect(EnemyStat statCategory)
    {
        if (GetStatCount(statCategory) == 0) return;

        Status randomStat = statusEffects[statCategory].ElementAt(UnityEngine.Random.Range(0, GetStatCount(statCategory)));
        
        statusEffects[statCategory].Remove(randomStat);
    }

    public float GetStatModifier(EnemyStat enemyStat)
    {
        float mod = 1;

        foreach (Status status in statusEffects[enemyStat])
        {
            mod += status.modifier;
        }

        return mod;
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
