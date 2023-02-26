using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DragonTailAttackState", menuName = "Behavior/DragonTailAttackState")]
public class DragonTailAttackState : SpawnProjectileState
{
    protected override void Shoot(BehaviorStateManager manager)
    {
        manager.SetAnimation(EnemyAnimationTriggers.Tail);

        DragonTail tail = (DragonTail)GetProjectile(manager);
        Vector2 tailOrigin = manager.GetTargetPosition();
        tail.transform.position = new Vector3(tailOrigin.x + firePosition.x, firePosition.y, tail.transform.position.z);
        tail.SetProjectile(Vector3.zero, projectileDamage * manager.GetDamageModifier(), projectileSpeed);
        tail.StartWindUp();

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
}
