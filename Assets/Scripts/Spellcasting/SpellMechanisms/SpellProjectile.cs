using System;
using UnityEngine;

/// <summary>
/// Manages the actual projectile that is launched from a spell
/// </summary>
public class SpellProjectile : MonoBehaviour {
        //Settings
        private bool homing;
        private float speed;
        private float lifetime; //measured in seconds (before projectile dies)
        private EnemyHealth enemy;
        private Action<EnemyHealth> onHit; //calls this function when the target is hit

        //Data
        private float elapsed; // how long it's lived

        public void Initialize(bool homing, float speed, float lifetime, EnemyHealth enemy, Action<EnemyHealth> onHit) {
                this.homing = homing;
                this.speed = speed;
                this.lifetime = lifetime;
                this.enemy = enemy;
                this.onHit = onHit;
        }

        private void Start() {
                //aim the projectile at the target
                transform.LookAt(this.enemy.transform);
        }

        /// <summary>
        /// Handles the movement of the projectile
        /// </summary>
        private void Update() {
                //Yes we should be using Object Pooling but this is for the sake of time 
                //TODO: Expand this to use Object Pooling
                if (this.elapsed >= lifetime) {
                        Destroy(this);
                }

                //tracks the enemy
                if (this.homing) {
                        transform.LookAt(this.enemy.transform);
                }

                //move it forward
                transform.Translate(Vector3.forward * this.speed * Time.deltaTime);

                //increase its elapsed time
                this.elapsed += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
                EnemyHealth enemyHealth = null;
                if (collision.TryGetComponent<EnemyHealth>(out enemyHealth)) {
                        //Pass responsibility to SpellProjectileData
                        this.onHit.Invoke(enemyHealth);

                        Destroy(gameObject);    
                }
        }




}