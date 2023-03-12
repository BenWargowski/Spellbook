using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CircularShootProjectileState", menuName = "Behavior/CircularShootProjectileState")]
public class CircularShootProjectileState : ShootProjectileState
{
    [SerializeField] private int primaryProjectileCount;
    [SerializeField] private int secondaryProjectileCount;

    [SerializeField] private AudioClip shootClip;

    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Shoot);
        SoundManager.Instance.PlaySound(shootClip);

        int projectileCount = (currentCount % 2 == 0 ? primaryProjectileCount : secondaryProjectileCount);

        float degrees = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float xDirection = Mathf.Cos(Mathf.Deg2Rad * degrees * i);
            float yDirection = Mathf.Sin(Mathf.Deg2Rad * degrees * i);
            aimDirection = new Vector3(xDirection, yDirection, aimDirection.z).normalized;
            Vector3 projectileOrigin = manager.transform.position + new Vector3(aimDirection.x * firePosition.x, aimDirection.y * firePosition.y, 0);

            BasicProjectile projectile = GetProjectile(manager);
            projectile.transform.position = projectileOrigin;
            projectile.SetProjectile(aimDirection, projectileDamage * manager.GetDamageModifier(), projectileSpeed);
        }

        timeSinceFired = 0;

        currentCount++;
    }
}
