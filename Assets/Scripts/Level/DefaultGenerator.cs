using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The default level layout. Generates tiles in the shape of a keyboard using a formula.
/// </summary>
public class DefaultGenerator : MonoBehaviour, ILevelLoader {
    [Header("Options")]
    /*
        [Q][W][E][R][T][Y]...
         [A][S][D][F]...
          [Z][X]...
        
        Each row of keys is indented by a small space compared to the row above it on a real QWERTY keyboard.
        (Note the gap between left side of [Q] and [A])
        [Q]'s row will be this[0], [A] will be this[1], [Z] will be this[2]
    */
    [SerializeField] private float[] rowStartOffset;
    
    //Distance between keys on both horizontal and vertical axes
    [SerializeField] private Vector2 keyDistance;

    //Start Position (AKA: position of top left key [Q])
    [SerializeField] private Vector2 startPosition;

    //Should this spawn in the tiles too?    
    [SerializeField] private bool createTiles;

    [Header("References")]

    //Tiles to instantiate at the key locations
    [SerializeField] private GameObject tilePrefab;

    public Dictionary<char, Vector2> GetTilePositions() {
        Dictionary<char, Vector2> tileMap = new Dictionary<char, Vector2>();
        InitializeTilePositions(ref tileMap);

        if (this.createTiles) InstantiateTiles(tileMap);
        return tileMap;
    }

    /// <summary>
    /// Populates the dictionary with the desired key positions
    /// </summary>
    private void InitializeTilePositions(ref Dictionary<char, Vector2> tilePositions) {
        string[] keyboard = {
            "QWERTYUIOP",
            "ASDFGHJKL",
            "ZXCVBNM"
        };

        //Addressing keyboard keys by keyboard[row][col]
        for (int row = 0; row < keyboard.Length; ++row) {
            for (int col = 0; col < keyboard[row].Length; ++col) {
                tilePositions[keyboard[row][col]] = new Vector2(
                    //horizontal
                    (
                        //first start at the desired start position
                        //then move the desired offset for the given row
                        //then travel along, adding 1 keyDistance for each key
                        startPosition.x + (rowStartOffset[row]) + (col * keyDistance.x)
                    ),
                    //vertical
                    (
                        startPosition.y - (row * keyDistance.y)
                    )
                );
            }
        }
    }

    /// <summary>
    /// Instantiates a tile GameObject from the prefab at each position
    /// </summary>
    private void InstantiateTiles(in Dictionary<char, Vector2> tilePositions) {
        if (this.tilePrefab == null) return;

        foreach (char c in tilePositions.Keys) {
            Instantiate(this.tilePrefab, tilePositions[c], Quaternion.identity, this.transform);
        }
    }
}
