using System.Collections.Generic;
using UnityEngine;

public class StageLayout : MonoBehaviour {

    //Singleton reference (feel free to refactor this out)
    public static StageLayout Instance {get; private set;}

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
    
    //Stores the positions of each 'key' / tile that the player can stand on
    public Dictionary<char, Vector2> TilePositions {get; private set;}

    private void Awake() {
        //Setting singleton reference
        if (Instance != null) Destroy(this);
        Instance = this;

        this.TilePositions = new Dictionary<char, Vector2>();
        InitializeTilePositions();
        InstantiateTiles();
    }

    /// <summary>
    /// Populates the dictionary with the desired key positions
    /// </summary>
    private void InitializeTilePositions() {
        string[] keyboard = {
            "QWERTYUIOP",
            "ASDFGHJKL",
            "ZXCVBNM"
        };

        //Addressing keyboard keys by keyboard[row][col]
        for (int row = 0; row < keyboard.Length; ++row) {
            for (int col = 0; col < keyboard[row].Length; ++col) {
                this.TilePositions[keyboard[row][col]] = new Vector2(
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
    private void InstantiateTiles() {
        if (this.tilePrefab == null) return;

        foreach (char c in this.TilePositions.Keys) {
            Instantiate(this.tilePrefab, this.TilePositions[c], Quaternion.identity, this.transform);
        }
    }

}