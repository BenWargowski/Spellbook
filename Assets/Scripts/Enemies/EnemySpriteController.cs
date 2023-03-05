using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteController : EnemyDamagedVisual
{
    protected BehaviorStateManager behaviorManager;
    private EnemyStatusManager statusManager;
    private EnemyHealth health;
    protected bool isActive = true;

    protected override void Start()
    {
        base.Start();

        GameEvents.Instance.playerVictory += Deactivate;

        behaviorManager = GetComponentInParent<BehaviorStateManager>();

        behaviorManager.onActionLocked += Deactivate;
        behaviorManager.onActionUnlocked += Activate;

        statusManager = GetComponentInParent<EnemyStatusManager>();
        statusManager.onStunned += Deactivate;
        statusManager.onNotStunned += Activate;

        health = GetComponentInParent<EnemyHealth>();
    }

    void LateUpdate()
    {
        if (!isActive) return;

        if (behaviorManager.GetIsFacingRight())
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }
    }

    private void Activate()
    {
        if (health.Health <= 0) return;

        isActive = true;
    }

    private void Deactivate()
    {
        isActive = false;
    }
}
