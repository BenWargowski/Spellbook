using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ConedShootProjectileState", menuName = "Behavior/ConedShootProjectileState")]
public class ConedShootProjectileState : ShootProjectileState
{
    [SerializeField] private float coneDegAngle;

    [SerializeField] private float windUp;

    [SerializeField] private ParticleSystem windUpParticles;

    private ParticleSystem particleSystemInstance;
    private float timeSinceEntered;

    public override void EnterState(BehaviorStateManager manager)
    {
        manager.LockAction();

        aimDirection = (manager.GetTargetPosition() - new Vector2(manager.transform.position.x, manager.transform.position.y)).normalized;

        timeSinceEntered = 0f;

        if (particleSystemInstance == null)
        {
            particleSystemInstance = Instantiate(windUpParticles).GetComponent<ParticleSystem>();
        }
        particleSystemInstance.transform.position = manager.transform.position + new Vector3(aimDirection.x * firePosition.x, aimDirection.y * firePosition.y, 0);
        particleSystemInstance.Play();

        base.EnterState(manager);
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (currentCount >= maxProjectileFire)
        {
            aimDirection = (manager.GetTargetPosition() - new Vector2(manager.transform.position.x, manager.transform.position.y)).normalized;
            particleSystemInstance.transform.position = manager.transform.position + new Vector3(aimDirection.x * firePosition.x, aimDirection.y * firePosition.y, 0);
        }

        if (timeSinceEntered < windUp)
        {
            timeSinceEntered += Time.deltaTime;
            return;
        }

        base.UpdateState(manager);
    }

    public override void ExitState(BehaviorStateManager manager)
    {
        particleSystemInstance.Stop();
        manager.UnlockAction();

        base.ExitState(manager);
    }

    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Shoot);

        Vector3 conedAimDirection = Quaternion.Euler(0, 0, Random.Range(-coneDegAngle, coneDegAngle)) * aimDirection;

        Vector3 projectileOrigin = manager.transform.position + new Vector3(aimDirection.x * firePosition.x, aimDirection.y * firePosition.y, 0);

        BasicProjectile projectile = GetProjectile(manager);
        projectile.transform.position = projectileOrigin;
        projectile.SetProjectile(conedAimDirection, projectileDamage * manager.GetDamageModifier(), projectileSpeed);

        timeSinceFired = 0;

        currentCount++;

        if (currentCount >= maxProjectileFire)
        {
            particleSystemInstance.Stop();
            manager.UnlockAction();
        }
    }
}
