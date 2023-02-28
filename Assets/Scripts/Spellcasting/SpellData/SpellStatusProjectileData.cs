using UnityEngine;

/// <summary>
/// Holds data for Spells that deal damage on contact and also inflict a Status Effect
/// </summary>
[CreateAssetMenu(fileName = "New SpellStatusProjectile", menuName = "Spells/SpellStatusProjectile")]
public class SpellStatusProjectileData : SpellProjectileData {
        [Header("Status Effect Info")]
        [SerializeField] protected EnemyStat stat;
        [SerializeField] protected float effectDuration;
        [SerializeField] protected float effectModifier;

        public override void OnHit(EnemyHealth enemyHealth) {
                //Deal damage as normal
                base.OnHit(enemyHealth);

                //Apply specified effect
                EnemyStatusManager statusManager = null;
                if (enemyHealth.TryGetComponent<EnemyStatusManager>(out statusManager)) {
                        statusManager.AddStatusEffect(this.stat, new Status(this.effectModifier, this.effectDuration));
                }
        }
}