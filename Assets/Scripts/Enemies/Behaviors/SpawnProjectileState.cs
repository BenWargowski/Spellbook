using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SpawnProjectileState", menuName = "Behavior/SpawnProjectileState")]
public class SpawnProjectileState : ShootProjectileState
{
    [SerializeField] private float yOriginOffset;

    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Shoot);

        BasicProjectile projectile = GetProjectile(manager);
        Vector2 projectileOrigin = manager.GetTargetPosition();
        projectile.transform.position = new Vector3(projectileOrigin.x, projectileOrigin.y + yOriginOffset, projectile.transform.position.z);
        projectile.SetProjectile(Vector3.zero, projectileDamage * manager.GetDamageModifier(), projectileSpeed);

        timeSinceFired = 0;

        currentCount++;
    }
}
