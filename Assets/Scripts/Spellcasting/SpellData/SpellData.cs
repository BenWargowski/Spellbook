using UnityEngine;

/// <summary>
/// Abstract class to hold general spell data. Other spells should derive from this.
/// </summary>
public abstract class SpellData : ScriptableObject {
        [SerializeField] protected string spellName;
        [SerializeField] protected int manaCost;
        [SerializeField] protected int cooldown;
        [SerializeField] protected string playerAnimationName;

        //Properties to access fields
        public string SpellName => spellName;
        public float Cooldown => cooldown;

        public virtual bool CastSpell(Player player) {
                //Make sure the player has enough Mana
                if (player.Mana < this.manaCost) return false;

                //Subtract mana
                player.Mana -= this.manaCost;

                //Plays animation
                if (this.playerAnimationName != null) {
                        Animator animator = null;
                        if (player.TryGetComponent<Animator>(out animator)) {
                                animator.Play(this.playerAnimationName);
                        }
                }

                return true;
        }
}