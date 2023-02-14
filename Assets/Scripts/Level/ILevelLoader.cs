using System.Collections.Generic;
using UnityEngine;

public interface ILevelLoader {
    public abstract Dictionary<char, GameObject> GetTiles();
}