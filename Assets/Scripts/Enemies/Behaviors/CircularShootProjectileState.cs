using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CircularShootProjectileState", menuName = "Behavior/CircularShootProjectileState")]
public class CircularShootProjectileState : ShootProjectileState
{
    [SerializeField] private int primaryProjectileCount;
    [SerializeField] private int secondaryProjectileCount;

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (manager.GetIsMoving()) return;

        timeSinceFired += Time.deltaTime;

        if (timeSinceFired >= fireRate)
        {
            Shoot(manager);
        }

        if (currentCount >= maxProjectileFire)
        {
            manager.ChangeState(returningState);
        }
    }

    private void Shoot(BehaviorStateManager manager)
    {
        int projectileCount = (currentCount % 2 == 0 ? primaryProjectileCount : secondaryProjectileCount);

        float degrees = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float xDirection = Mathf.Cos(Mathf.Deg2Rad * degrees * i);
            float yDirection = Mathf.Sin(Mathf.Deg2Rad * degrees * i);
            aimDirection = new Vector3(xDirection, yDirection, aimDirection.z);

            Vector3 projectileOrigin = manager.transform.position;
            BasicProjectile projectile = GetProjectile(manager);
            projectile.transform.position = projectileOrigin;
            projectile.SetProjectile(aimDirection, damage * manager.GetDamageModifier());
        }

        timeSinceFired = 0;

        currentCount++;
    }
}
