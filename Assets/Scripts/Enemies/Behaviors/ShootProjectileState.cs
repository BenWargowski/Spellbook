using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShootProjectileState", menuName = "Behavior/ShootProjectileState")]
public class ShootProjectileState : BehaviorState
{
    [SerializeField] private BehaviorState returningState;

    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private int maxProjectileCount;

    [SerializeField] private float fireRate;

    [SerializeField] private Vector3 aimDirection;

    [System.NonSerialized] private List<BasicProjectile> projectilePool = new List<BasicProjectile>();

    private float timeSinceFired;

    private int currentCount;

    public override void EnterState(BehaviorStateManager manager)
    {
        timeSinceFired = 0;

        currentCount = 0;
    }

    public override void UpdateState(BehaviorStateManager manager)
    {
        timeSinceFired += Time.deltaTime;

        if (timeSinceFired >= fireRate)
        {
            Shoot(manager);
        }

        if (currentCount >= maxProjectileCount)
        {
            manager.ChangeState(returningState);
        }
    }

    public override void OnStateTriggerEnter(BehaviorStateManager manager, Collider2D other)
    {
    }

    public override void OnStateTriggerExit(BehaviorStateManager manager, Collider2D other)
    {
    }

    private void Shoot(BehaviorStateManager manager)
    {
        Vector3 targetPosition = manager.GetTargetPosition();
        aimDirection = (targetPosition - manager.transform.position).normalized;

        BasicProjectile projectile = GetProjectile(manager);
        projectile.transform.position = manager.transform.position;
        projectile.SetDirection(aimDirection);


        timeSinceFired = 0;

        currentCount++;
    }

    private BasicProjectile GetProjectile(BehaviorStateManager manager)
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
}
