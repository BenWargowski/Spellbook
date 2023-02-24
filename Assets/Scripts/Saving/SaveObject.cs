using System.Collections.Generic;

/// <summary>
/// Class/struct to save game progression
/// </summary>
[System.Serializable]
public class SaveObject {
    /// <summary>
    /// List of spell names that the player can use (has unlocked).
    /// </summary>
    public List<string> unlockedSpells;

    /// <summary>
    /// Highest Level / Boss Stage unlocked.
    /// This current level has not been beaten, and all levels under have been.
    /// </summary>
    public int highestBossLevel;
}
