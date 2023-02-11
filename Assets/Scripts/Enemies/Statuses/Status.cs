using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public float modifier { get; private set; }

    public float timeRemaining { get; protected set; }

    public Status(float modifierChange, float duration)
    {
        modifier = modifierChange;

        timeRemaining = duration;
    }

    public void UpdateStatus()
    {
        timeRemaining -= Time.deltaTime;
    }

    public bool IsValid()
    {
        return timeRemaining > 0;
    }
}
