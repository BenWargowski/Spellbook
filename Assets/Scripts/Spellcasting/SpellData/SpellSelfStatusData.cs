using UnityEngine;

/// <summary>
/// Spells that apply a status effect to the player
/// </summary>

[CreateAssetMenu(fileName = "New SpellSelfStatus", menuName = "Spells/SpellSelfStatus")]
public class SpellSelfStatusData : SpellData {
        [Header("Status Effect Info")]
        [SerializeField] protected PlayerStat stat;
        [SerializeField] protected float effectDuration;
        [SerializeField] protected float effectModifier;

        public override bool CastSpell(Player player) {
                //Manage mana
                if (!base.CastSpell(player)) return false;

                //Apply the status effect
                player.AddStatusEffect(this.stat, new Status(this.effectModifier, this.effectDuration));
                return true;
        }
}