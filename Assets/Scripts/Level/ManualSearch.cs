using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Loader that searches for pre-existing tiles with the Tile tag
/// with the name starting with the prefix and ending in the required character.
/// </summary>
public class ManualSearch : MonoBehaviour, ILevelLoader{

    [Header("Options")]
    [SerializeField] private string prefix;

    public Dictionary<char, Vector2> GetTilePositions() {
        Dictionary<char, Vector2> tileMap = new Dictionary<char, Vector2>();

        GameObject[] keyTiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in keyTiles) {
            //If name starts with prefix, get last char of name and record the position of this tile under that char
            if (string.IsNullOrEmpty(prefix) || tile.name.StartsWith(this.prefix)) {
                tileMap[Char.ToUpper(tile.name[tile.name.Length - 1])] = tile.transform.position;
            }
        }

        return tileMap;
    }
}
