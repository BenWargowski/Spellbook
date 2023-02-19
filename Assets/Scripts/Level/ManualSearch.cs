using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Level Loader that searches for pre-existing tiles with the Tile tag
/// with the name starting with the prefix and ending in the required character.
/// </summary>
public class ManualSearch : MonoBehaviour, ILevelLoader {

    [Header("Options")]
    [SerializeField] private string prefix;
    [SerializeField] private bool ignoreCount;

    public Dictionary<char, GameObject> GetTiles() {
        Dictionary<char, GameObject> tileMap = new Dictionary<char, GameObject>();

        GameObject[] keyTiles = GameObject.FindGameObjectsWithTag("Tile");
        //either too many or too little tiles (should be one tile per English letter, so 26)
        if (!ignoreCount && keyTiles.Length != 26) {
            throw new System.Exception($"ManualSearch: Incorrect Tile Count! Expected 26, Got {keyTiles.Length}.");
        }

        foreach (GameObject tile in keyTiles) {
            //If name starts with prefix, get last char of name and record the position of this tile under that char
            if (string.IsNullOrEmpty(prefix) || tile.name.StartsWith(this.prefix)) {
                char c = tile.name[tile.name.Length - 1];
                if (Char.IsLetter(c)) {
                    c = Char.ToUpper(c);

                    //set text display on tile
                    TextMeshPro text = null;
                    if (tile.transform.GetChild(0).GetChild(0).TryGetComponent<TextMeshPro>(out text)) {
                        text.text = "" + c;
                    } 

                    tileMap[c] = tile;
                }
            }
        }

        return tileMap;
    }
}
