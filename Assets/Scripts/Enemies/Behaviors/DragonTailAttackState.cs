using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DragonTailAttackState", menuName = "Behavior/DragonTailAttackState")]
public class DragonTailAttackState : SpawnProjectileState
{
    [SerializeField] List<Vector2> spawnLocations;

    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Tail);

        BasicProjectile tail = GetProjectile(manager);
        Vector2 tailOrigin = GetSpawnLocation(manager.GetTargetPosition());
        tail.transform.position = new Vector3(tailOrigin.x + firePosition.x, tailOrigin.y + firePosition.y, tail.transform.position.z);
        tail.SetProjectile(Vector3.zero, projectileDamage * manager.GetDamageModifier(), projectileSpeed);

        timeSinceFired = 0;

        currentCount++;
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

    private Vector2 GetSpawnLocation(Vector2 target)
    {
        Vector2 spawnLocation = spawnLocations[0];

        for (int i = 0; i < spawnLocations.Count; i++)
        {
            if (Vector2.Distance(spawnLocations[i], target) < Vector2.Distance(spawnLocation, target))
                spawnLocation = spawnLocations[i];
        }

        return spawnLocation;
    }
}
