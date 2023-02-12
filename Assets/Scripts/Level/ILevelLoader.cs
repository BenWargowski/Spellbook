using System.Collections.Generic;
using UnityEngine;

public interface ILevelLoader {
    public abstract Dictionary<char, Vector2> GetTilePositions();
}