/// <summary>
/// Different player stats that Status Effects can change.
/// </summary>
public enum PlayerStat {
    MAX_HEALTH,
    MAX_MANA,
    MOVEMENT_SPEED,
    SPELL_DAMAGE,
    ARMOR,


    // OTHER is a special stat.
    // This does not directly affect any stat,
    // so it can be used as a "dummy" for custom effects.
    OTHER,
}
