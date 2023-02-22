using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MassSpawnProjectileState", menuName = "Behavior/MassSpawnProjectileState")]
public class MassSpawnProjectileState : SpawnProjectileState
{
    [SerializeField] private int tilesPerBurst;

    private char[] keyTiles =
        {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J',
    'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
    'V', 'W', 'X', 'Y', 'Z'};

    protected override void Shoot(BehaviorStateManager manager)
    {
        List<char> tiles = new List<char>(keyTiles);
        List<Vector2> targetPositions = new List<Vector2>();

        for (int i = 0; i < tilesPerBurst; i++)
        {
            char tile = tiles[Random.Range(0, tiles.Count)];
            tiles.Remove(tile);
            targetPositions.Add(StageLayout.Instance.TilePositions[tile]);
        }

        manager.SetAnimation(EnemyAnimationTriggers.Shoot);

        foreach (Vector2 target in targetPositions)
        {
            BasicProjectile projectile = GetProjectile(manager);
            Vector2 projectileOrigin = target;
            projectile.transform.position = new Vector3(projectileOrigin.x, projectileOrigin.y + yOriginOffset, projectile.transform.position.z);
            projectile.SetProjectile(Vector3.zero, projectileDamage * manager.GetDamageModifier(), projectileSpeed);
        }

        timeSinceFired = 0;

        currentCount++;
    }

    public override bool EnterCondition(BehaviorStateManager manager)
    {
        return Random.Range(0, 4) == 0 ? true : false;
    }
}
