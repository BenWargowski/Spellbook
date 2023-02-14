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

    [Header("References")]

    //Tiles to instantiate at the key locations
    [SerializeField] private GameObject tilePrefab;

    public Dictionary<char, GameObject> GetTiles() {
        Dictionary<char, GameObject> tiles = new Dictionary<char, GameObject>();

        string[] keyboard = {
            "QWERTYUIOP",
            "ASDFGHJKL",
            "ZXCVBNM"
        };

        //Addressing keyboard keys by keyboard[row][col]
        for (int row = 0; row < keyboard.Length; ++row) {
            for (int col = 0; col < keyboard[row].Length; ++col) {
                tiles[keyboard[row][col]] = Instantiate(
                    this.tilePrefab,
                    new Vector2 (
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
                    ),
                    Quaternion.identity,
                    this.transform
                );
            }
        }

        return tiles;
    }
}
