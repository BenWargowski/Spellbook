using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level Loader that searches for pre-existing tiles with the Tile tag
/// and attempts to determine which tile is which key automatically
/// </summary>
public class AutoSearch : MonoBehaviour, ILevelLoader{
    public Dictionary<char, Vector2> GetTilePositions() {
        Dictionary<char, Vector2> tileMap = new Dictionary<char, Vector2>();
        string[] keyboard = {
            "QWERTYUIOP",
            "ASDFGHJKL",
            "ZXCVBNM"
        };

        //TODO: CLEAN UP THIS CODE

        HashSet<GameObject> keyTiles = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Tile"));

        for (int a = 0; a < keyboard.Length; ++a) {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < keyboard[a].Length; ++i) {
                GameObject temp = null;
                float y = float.MinValue;

                foreach (GameObject tile in keyTiles) {
                    if (tile.transform.position.y > y) {
                        temp = tile;
                        y = tile.transform.position.y;
                    }
                }

                if (temp != null) {
                    list.Add(temp);
                    keyTiles.Remove(temp);
                }
            }

            list.Sort(Comparer<GameObject>.Create(
                (x, y) => x.transform.position.x > y.transform.position.x ? 1 : x.transform.position.x < y.transform.position.x ? -1 : 0
            ));
            for (int i = 0; i < keyboard[a].Length; ++i) {
               tileMap[keyboard[a][i]] = list[i].transform.position;
            }
        }

        return tileMap;
    }
}
