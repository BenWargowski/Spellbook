using UnityEngine;

/// <summary>
/// Spells that deal an instant Area of Effect Damage and also apply a Status Effect
/// </summary>
[CreateAssetMenu(fileName = "New SpellAOEStatusDamage", menuName = "Spells/SpellAOEStatusDamage")]
public class SpellAOEStatusDamageData : SpellAOEDamageData {
        [Header("Status Effect Info")]
        [SerializeField] protected EnemyStat stat;
        [SerializeField] protected float effectDuration;
        [SerializeField] protected float effectModifier;

        protected override void OnHit(EnemyHealth enemyHealth, Player player) {
                //still damage it
                base.OnHit(enemyHealth, player);

                //Apply specified effect
                EnemyStatusManager statusManager = null;
                if (enemyHealth.TryGetComponent<EnemyStatusManager>(out statusManager)) {
                        statusManager.AddStatusEffect(this.stat, new Status(this.effectModifier, this.effectDuration));
                }
        }
}