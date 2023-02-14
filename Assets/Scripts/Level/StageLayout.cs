using System;
using System.Collections.Generic;
using UnityEngine;

public class StageLayout : MonoBehaviour {
    //Singleton reference (feel free to refactor this out)
    public static StageLayout Instance {get; private set;}
    
    [SerializeField] private DefaultGenerator defaultGenerator;
    [SerializeField] private ManualSearch manualSearch;
    [SerializeField] private AutoSearch autoSearch;

    private ILevelLoader loader;

    //Stores the positions of each 'key' / tile that the player can stand on
    public Dictionary<char, GameObject> Tiles {get; private set;}

    public Dictionary<char, Vector2> TilePositions {get; private set;} //DEPRECATED

    private void Awake() {
        //Setting singleton reference
        if (Instance != null) Destroy(this);
        Instance = this;

        FindLoader();
        this.Tiles = loader.GetTiles();
        
        //Temporary patch to ensure Deprecated TilePositions still works
        TilePositions = new Dictionary<char, Vector2>();
        foreach (char c in Tiles.Keys) {
            TilePositions[c] = Tiles[c].transform.position;
        }
    }

    public Vector2 GetTilePosition(char c) {
        return Tiles[c].transform.position;
    }

    private void FindLoader() {
        if (defaultGenerator != null) loader = defaultGenerator;
        else if (manualSearch != null) loader = manualSearch;
        else if (autoSearch != null) loader = autoSearch;

        else {
            throw new System.Exception("StageLayout: No Generator provided!");
        }
    }
}
