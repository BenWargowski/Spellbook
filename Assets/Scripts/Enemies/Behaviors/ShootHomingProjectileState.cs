using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShootHomingProjectileState", menuName = "Behavior/ShootHomingProjectileState")]
public class ShootHomingProjectileState : ShootProjectileState
{
    [SerializeField] private float projectileRotationSpeed;

    public override void UpdateState(BehaviorStateManager manager)
    {
        base.UpdateState(manager);

        if (timeSinceReseting < windDown / 2)
        {
            for (int i = 0; i < projectilePool.Count; i++)
            {
                HomingProjectile projectile = (HomingProjectile)projectilePool[i];
                if (projectile.gameObject.activeSelf)
                    projectile.UpdateDirection((manager.GetTargetPosition() - new Vector2(projectile.transform.position.x, projectile.transform.position.y)).normalized);
            }

        }
    }

    public override bool EnterCondition(BehaviorStateManager manager)
    {
        CheckProjectilePool();

        int activeProjectileCount = 0;

        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i].gameObject.activeSelf)
                activeProjectileCount++;
        }

        return activeProjectileCount == 0;
    }

    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Shoot);

        aimDirection = (manager.GetTargetPosition() - new Vector2(manager.transform.position.x, manager.transform.position.y)).normalized;
        Vector3 projectileOrigin = manager.transform.position + new Vector3(aimDirection.x * firePosition.x, aimDirection.y * firePosition.y, 0);

        HomingProjectile projectile = (HomingProjectile)GetProjectile(manager);
        projectile.transform.position = projectileOrigin;
        projectile.SetProjectile(aimDirection, projectileDamage * manager.GetDamageModifier(), projectileSpeed);
        projectile.SetRotationSpeed(projectileRotationSpeed);

        timeSinceFired = 0;

        currentCount++;
    }
}
