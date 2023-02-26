using UnityEngine;

/// <summary>
/// Holds data for spells that fire projectiles that deal damage on contact
/// </summary>
[CreateAssetMenu(fileName = "New SpellProjectile", menuName = "Spells/SpellProjectile")]
public class SpellProjectileData : SpellData {
        [Header("References")]
        [SerializeField] protected GameObject prefab;

        [Header("Projectile Info")]
        [SerializeField] protected bool homing;
        [SerializeField] protected float projectileSpeed;
        [SerializeField] protected float projectileLifetime;
        [SerializeField] protected float damage;
        [SerializeField] protected SpellElement element;

        protected Player player;

        public override bool CastSpell(Player player) {
                this.player = player;

                //Manage mana
                if (!base.CastSpell(player)) return false;

                //Search for the enemy
                //TODO: not the greatest implementation
                EnemyHealth enemy = FindObjectOfType<EnemyHealth>();
                if (enemy == null) {
                        Debug.LogWarning("Enemy not found! Projectile terminating.");
                        return false;
                }

                //Yes we should be using Object Pooling but this is for the sake of time 
                //TODO: Expand this to use Object Pooling
                Vector2 direction = enemy.transform.position - player.transform.position;
                GameObject projectileObject = Instantiate(this.prefab, player.transform.position, Quaternion.LookRotation(direction, Vector2.up));

                SpellProjectile projectile = null;
                if (!projectileObject.TryGetComponent<SpellProjectile>(out projectile)) {
                        //Not found -- something went wrong!
                        Debug.LogWarning("SpellProjectile Component Not Found!");
                        return false;
                }
                
                projectile.Initialize(homing, projectileSpeed, projectileLifetime, enemy, OnHit);
                return true;
        }

        /// <summary>
        /// The Spell Projectile calls this function to hit the enemy. This can be overriden in derived classes.
        /// </summary>
        /// <param name="enemyHealth">Enemy to hit</param>
        public virtual void OnHit(EnemyHealth enemyHealth) {
                enemyHealth.Damage(this.damage * this.player.SpellDamageMultiplier, this.element, false);
        }
}