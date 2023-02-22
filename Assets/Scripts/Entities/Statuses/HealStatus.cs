using UnityEngine;

/// <summary>
/// Status that can be used for healing/regenerating over time
/// Status Type: OTHER
/// </summary>
public class HealStatus : Status { //Inherits from Enemy Status script
    //Whatever entity that can be healed
    private IHealable entity;

    /// <param name="hp">Amount of health to heal per second.
    /// <param name="duration">Duration in seconds</param>
    public HealStatus(IHealable entity, float hp, float duration) : base(hp, duration) {
        this.entity = entity;
    }

    public override void UpdateStatus() {
        base.UpdateStatus();

        //Apply health change at correct rate
        this.entity.Heal(this.modifier * Time.deltaTime);
    }

}
