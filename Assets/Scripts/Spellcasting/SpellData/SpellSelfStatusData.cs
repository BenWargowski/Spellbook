using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Spells that apply a status effect to the player
/// </summary>

[CreateAssetMenu(fileName = "New SpellSelfStatus", menuName = "Spells/SpellSelfStatus")]
public class SpellSelfStatusData : SpellData {
        [Header("Status Effect Info")]
        [SerializeField] protected PlayerStat stat;
        [SerializeField] protected float effectDuration;
        [SerializeField] protected float effectModifier;
        [SerializeField] protected bool heal;
        
        public override bool CastSpell(Player player) {
                //Manage mana
                if (!base.CastSpell(player)) return false;

                //Apply the status effect
                Status status = null;
                if (this.heal) {
                        status = new HealStatus(player, this.effectModifier, this.effectDuration);
                }
                else {
                        status = new Status(this.effectModifier, this.effectDuration);
                }

                player.AddStatusEffect(this.stat, status);
                return true;
        }
}