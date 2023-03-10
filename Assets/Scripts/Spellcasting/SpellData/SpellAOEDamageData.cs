using UnityEngine;

/// <summary>
/// Spells that deal an instant Area Of Effect Damage
/// </summary>
[CreateAssetMenu(fileName = "New SpellAOEDamage", menuName = "Spells/SpellAOEDamage")]
public class SpellAOEDamageData : SpellData {
        [Header("Projectile Info")]
        [SerializeField] protected LayerMask enemyLayer;
        [SerializeField] protected float radius;
        [SerializeField] protected float damage;
        [SerializeField] protected SpellElement element;

        public override bool CastSpell(Player player) {
                //Handle Mana
                if (!base.CastSpell(player)) return false;

                //checks for all enemies within a circle
                Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, this.radius, this.enemyLayer);
                foreach (Collider2D collider in colliders) {
                        EnemyHealth enemyHealth = null;
                        if (collider.TryGetComponent<EnemyHealth>(out enemyHealth)) {
                                OnHit(enemyHealth, player);
                        }
                }

                return true;
        }

        protected virtual void OnHit(EnemyHealth enemyHealth, Player player) {
                enemyHealth.Damage(this.damage * player.SpellDamageMultiplier, this.element, false);
        }
}