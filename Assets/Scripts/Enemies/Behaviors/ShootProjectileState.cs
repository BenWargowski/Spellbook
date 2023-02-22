using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "New ShootProjectileState", menuName = "Behavior/ShootProjectileState")]
public class ShootProjectileState : BehaviorState
{
    [SerializeField] protected BehaviorState returningState;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] protected int maxProjectileFire;

    [SerializeField] protected float fireRate;

    [SerializeField] protected float projectileDamage;

    [SerializeField] protected float projectileSpeed;

    [SerializeField] protected Vector2 firePosition;

    [SerializeField] protected float windDown;

    protected Vector3 aimDirection;

    [System.NonSerialized] protected List<BasicProjectile> projectilePool = new List<BasicProjectile>();

    protected float timeSinceFired;
    protected float timeSinceReseting;
    protected int currentCount;

    public override void EnterState(BehaviorStateManager manager)
    {
        CheckProjectilePool();

        timeSinceFired = fireRate;

        currentCount = 0;

        timeSinceReseting = 0;
    }

    public override void ExitState(BehaviorStateManager manager)
    {

    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        if (manager.GetIsMoving()) return;

        timeSinceFired += Time.deltaTime;


        if (currentCount < maxProjectileFire)
        {
            if (timeSinceFired >= fireRate)
                Shoot(manager);
        }
        else
        {
            if (timeSinceReseting >= windDown)
                manager.ChangeState(returningState);
            else
            {
                timeSinceReseting += Time.deltaTime;
            }
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {

    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {

    }

    protected virtual void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Shoot);

        Vector3 projectileOrigin = manager.transform.position + new Vector3((manager.GetIsFacingRight() ? 1 : -1) * firePosition.x, firePosition.y, 0);
        aimDirection = (manager.GetTargetPosition() - new Vector2(projectileOrigin.x, projectileOrigin.y)).normalized;

        BasicProjectile projectile = GetProjectile(manager);
        projectile.transform.position = projectileOrigin;
        projectile.SetProjectile(aimDirection, projectileDamage * manager.GetDamageModifier(), projectileSpeed);

        timeSinceFired = 0;

        currentCount++;
    }

    protected BasicProjectile GetProjectile(BehaviorStateManager manager)
    {
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (!projectilePool[i].gameObject.activeSelf)
            {
                projectilePool[i].gameObject.SetActive(true);

                return projectilePool[i];
            }
        }

        BasicProjectile newProjectile = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity).GetComponent<BasicProjectile>();

        projectilePool.Add(newProjectile);

        return newProjectile;
    }

    private void CheckProjectilePool()
    {
        List<BasicProjectile> newPool = new List<BasicProjectile>();

        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (projectilePool[i] != null)
                newPool.Add(projectilePool[i]);
        }

        projectilePool = newPool;
    }
}
