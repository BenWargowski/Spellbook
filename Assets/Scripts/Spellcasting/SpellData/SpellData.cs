using UnityEngine;

/// <summary>
/// Abstract class to hold general spell data. Other spells should derive from this.
/// </summary>
public abstract class SpellData : ScriptableObject {
        [Header("Generic Spell Info")]
        [SerializeField] protected string spellName;
        [SerializeField] protected int manaCost;
        [SerializeField] protected int cooldown;

        [Header("VFX/SFX")]
        [SerializeField] protected string playerAnimationName;
        [SerializeField] protected GameObject effectsPrefab;
        [SerializeField] protected SpellEffectLocation spellEffectLocation;

        //Properties to access fields
        public string SpellName => spellName;
        public float Cooldown => cooldown;
        public int ManaCost => manaCost;

        public virtual bool CastSpell(Player player) {
                //Make sure the player has enough Mana
                if (player.Mana < this.manaCost) return false;

                //Subtract mana
                player.Mana -= this.manaCost;

                //Plays player animation
                if (this.playerAnimationName != null) {
                        Animator animator = null;
                        if (player.TryGetComponent<Animator>(out animator)) {
                                animator.Play(this.playerAnimationName);
                        }
                }

                //Spawns effects prefab
                if (this.effectsPrefab != null) {
                        GameObject effects = Instantiate(this.effectsPrefab);
                        switch (spellEffectLocation) {
                                case SpellEffectLocation.PLAYER:
                                        effects.transform.position = player.transform.position;
                                        break;
                                case SpellEffectLocation.ENEMY:
                                        effects.transform.position = FindObjectOfType<EnemyHealth>().transform.position;
                                        break;
                                case SpellEffectLocation.STAGE_CENTER:
                                default:
                                        effects.transform.position = Vector3.zero;
                                        break;
                        }
                }

                return true;
        }
}