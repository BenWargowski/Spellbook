using UnityEngine;

/// <summary>
/// Status that can be used for inflicting damage over time
/// Status Type: OTHER
/// </summary>
public class DoTStatus : Status { //Inherits from Enemy Status script
    //Whatever entity that can be damaged
    private IDamageable entity;
    private bool ignoreArmor;

    /// <param name="dps">Amount of health to damage per second.
    /// <param name="duration">Duration in seconds</param>
    public DoTStatus(IDamageable entity, float dps, float duration, bool ignoreArmor) : base(dps, duration) {
        this.entity = entity;
        this.ignoreArmor = ignoreArmor;
    }

    public override void UpdateStatus() {
        base.UpdateStatus();

        //Apply health change at correct rate
        this.entity.Damage(this.modifier * Time.deltaTime, false, this.ignoreArmor);
    }

}
