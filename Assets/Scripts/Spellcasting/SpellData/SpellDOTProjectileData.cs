using UnityEngine;

/// <summary>
/// Holds data for Spells that deal damage on contact and also Damage Over Time
/// </summary>
[CreateAssetMenu(fileName = "New SpellDOTProjectile", menuName = "Spells/SpellDOTProjectile")]
public class SpellDOTProjectileData : SpellProjectileData {
        [Header("Damage Over Time Info")]
        [SerializeField] protected float dps;
        [SerializeField] protected float duration;

        public override void OnHit(EnemyHealth enemyHealth) {
                //Deal damage as normal
                base.OnHit(enemyHealth);

                //Apply a Damage Over Time Effect
                EnemyStatusManager statusManager = null;
                if (enemyHealth.TryGetComponent<EnemyStatusManager>(out statusManager)) {
                        statusManager.AddStatusEffect(EnemyStat.OTHER, new EnemyDoTStatus(enemyHealth, this.element, false, this.dps, this.duration));
                }
        }
}