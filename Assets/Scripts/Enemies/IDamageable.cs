/// <summary>
/// An entity that can be damaged
/// </summary>
public interface IDamageable {
    public void Damage(float damage, bool triggerInvuln, bool ignoreArmor);
}