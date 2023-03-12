using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MassSpawnProjectileState", menuName = "Behavior/MassSpawnProjectileState")]
public class MassSpawnProjectileState : SpawnProjectileState
{
    [SerializeField] private int tilesPerBurst;

    [SerializeField] private int guaranteedHitProportionChance;

    [SerializeField] private int enterConditionProportionChance;

    private char[] keyTiles =
        {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z'};

    protected override void Shoot(BehaviorStateManager manager)
    {
        List<char> tiles = new List<char>(keyTiles);
        List<Vector2> targetPositions = new List<Vector2>();

        char tile = ' ';
        for (int i = 0; i < tilesPerBurst; i++)
        {
            if (i == 0 && Random.Range(0, guaranteedHitProportionChance) == 0)
            {
                tile = FindTargetTile(manager, manager.GetTargetPosition(), 0f, .5f);
            }
            else
            {
                tile = tiles[Random.Range(0, tiles.Count)];
            }

            tiles.Remove(tile);
            targetPositions.Add(StageLayout.Instance.TilePositions[tile]);
        }
        
        manager.SetAnimation(EnemyAnimationTriggers.Spawn);

        foreach (Vector2 target in targetPositions)
        {
            BasicProjectile projectile = GetProjectile(manager);
            Vector2 projectileOrigin = target;
            projectile.transform.position = new Vector3(projectileOrigin.x + firePosition.x, projectileOrigin.y + firePosition.y, projectile.transform.position.z);
            projectile.SetProjectile(Vector3.zero, projectileDamage * manager.GetDamageModifier(), projectileSpeed);
        }

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

        return (Random.Range(0, enterConditionProportionChance) == 0 ? true : false) && activeProjectileCount == 0;
    }
}
