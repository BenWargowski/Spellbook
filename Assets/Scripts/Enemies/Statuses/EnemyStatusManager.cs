using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the enemy's damage modifier, speed modifier, and stunned status
/// </summary>
public class EnemyStatusManager : MonoBehaviour
{
    private List<Status> damageModifier = new List<Status>();

    private List<Status> speedModifier = new List<Status>();

    private List<Status> stunnedStatus = new List<Status>();

    [HideInInspector] public bool isInvincible = false;

    void Update()
    {
        damageModifier = UpdateStatus(damageModifier);
        speedModifier = UpdateStatus(speedModifier);
        stunnedStatus = UpdateStunnedStatus(stunnedStatus);
    }

    private List<Status> UpdateStatus(List<Status> statuses)
    {
        List<Status> validStatuses = new List<Status>();

        foreach (Status mod in statuses)
        {
            mod.UpdateStatus();

            if (mod.IsValid())
                validStatuses.Add(mod);
        }

        return validStatuses;
    }

    public event Action onNotStunned;
    private List<Status> UpdateStunnedStatus(List<Status> statuses)
    {
        List<Status> newStatuses = (isInvincible ? new List<Status>() : UpdateStatus(statuses));

        if (newStatuses.Count < statuses.Count && onNotStunned != null)
            onNotStunned();

        return newStatuses;
    }

    /// <summary>
    /// Use to add a damage modifier to the enemy
    /// </summary>
    /// <param name="change">Percentage-based change in enemy's attack damage. For example, change = -.1 will reduce enemy damage by 10%</param>
    /// <param name="duration">How long the added status will last</param>
    public void AddDamageMod(float change, float duration)
    {
        Status newStatus = new Status(change, duration);

        damageModifier.Add(newStatus);
    }

    /// <summary>
    /// Use to add a speed modifier to the enemy
    /// </summary>
    /// <param name="change">Percentage-based change in the enemy's move speed</param>
    /// <param name="duration">How long the added status will last</param>
    public void AddSpeedMod(float change, float duration)
    {
        Status newStatus = new Status(change, duration);

        speedModifier.Add(newStatus);
    }

    /// <summary>
    /// Use to stun the enemy for some duration in seconds
    /// </summary>
    public event Action onStunned;
    public void AddStun(float duration)
    {
        if (isInvincible) return;

        Status newStatus = new Status(0, duration);

        if (stunnedStatus.Count == 0)
        {
            stunnedStatus.Add(newStatus);

            if (onStunned != null)
                onStunned();
        }
        else
        {
            if (duration > stunnedStatus[0].timeRemaining)
                stunnedStatus[0] = newStatus;
        }
    }

    public float GetDamageMod()
    {
        float mod = 1f;

        foreach (Status status in damageModifier)
        {
            mod += status.modifier;
        }

        return mod;
    }

    public float GetSpeedMod()
    {
        float mod = 1f;

        foreach (Status status in damageModifier)
        {
            mod += status.modifier;
        }

        return mod;
    }
}
