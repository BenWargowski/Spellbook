using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Level Loader that searches for pre-existing tiles with the Tile tag
/// and attempts to determine which tile is which key automatically
/// </summary>
public class AutoSearch : MonoBehaviour, ILevelLoader {
    [SerializeField] private bool ignoreCount;

    public Dictionary<char, GameObject> GetTiles() {
        Dictionary<char, GameObject> tileMap = new Dictionary<char, GameObject>();
        int readIndex = 0;
        string[] keyboard = {
            "QWERTYUIOP",
            "ASDFGHJKL",
            "ZXCVBNM"
        };

        //Find tiles
        GameObject[] keyTiles = GameObject.FindGameObjectsWithTag("Tile");
        if (!ignoreCount && keyTiles.Length != 26) {
            throw new System.Exception($"AutoSearch: Incorrect Tile Count! Expected 26, Got {keyTiles.Length}.");
        }

        //Sort keyTiles by Y, decreasing
        Array.Sort(keyTiles, Comparer<GameObject>.Create(
            (x, y) => x.transform.position.y < y.transform.position.y ? 1 : x.transform.position.y > y.transform.position.y ? -1 : 0
        ));

        //Iterate through every row
        for (int r = 0; r < keyboard.Length; ++r) {
            GameObject[] keyRow = new GameObject[keyboard[r].Length];

            //The {col} next tiles with the highest Y position -> list
            //Essentially, grab an entire row's worth of keys from the topmost row
            for (int c = 0; c < keyboard[r].Length; ++c) {
                keyRow[c] = keyTiles[readIndex];
                ++readIndex;
            }

            //The list now contains a row of keys. Sort this so the one in the leftmost position is first.
            Array.Sort(keyRow, Comparer<GameObject>.Create(
                (x, y) => x.transform.position.x > y.transform.position.x ? 1 : x.transform.position.x < y.transform.position.x ? -1 : 0
            ));

            //Assign keys from left to right in this row
            for (int i = 0; i < keyboard[r].Length; ++i) {
                char c = keyboard[r][i];

                TextMeshPro text = null;
                if (keyRow[i].transform.GetChild(0).GetChild(0).TryGetComponent<TextMeshPro>(out text)) {
                    text.text = "" + c;
                } 

                tileMap[c] = keyRow[i];
            }
        }

        return tileMap;
    }
}
