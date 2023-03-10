using UnityEngine;

/// <summary>
/// Spell that instantly deals damage to the enemy when casted
/// </summary>
[CreateAssetMenu(fileName = "New SpellInstantDamage", menuName = "Spells/SpellInstantDamage")]
public class SpellInstantDamageData : SpellData {
        [Header("Damage Info")]
        [SerializeField] protected float damage;
        [SerializeField] protected SpellElement element;

        [Header("Status Effect Info")]
        [SerializeField] protected bool applyStatusEffect;
        [SerializeField] protected EnemyStat stat;
        [SerializeField] protected float effectDuration;
        [SerializeField] protected float effectModifier;

        public override bool CastSpell(Player player) {
                //handle mana
                if (!base.CastSpell(player)) return false;

                //Search for the enemy
                //TODO: not the greatest implementation
                EnemyHealth enemy = FindObjectOfType<EnemyHealth>();
                if (enemy == null) {
                        Debug.LogWarning("Enemy not found! Spell Instant Damage terminating.");
                        return false;
                }

                enemy.Damage(this.damage * player.SpellDamageMultiplier, this.element, false);

                //Apply status effect if specified
                if (this.applyStatusEffect) {
                        EnemyStatusManager statusManager = null;
                        if (enemy.TryGetComponent<EnemyStatusManager>(out statusManager)) {
                                statusManager.AddStatusEffect(this.stat, new Status(this.effectModifier, this.effectDuration));
                        }
                }

                return true;
        }

}