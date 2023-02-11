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
    public Dictionary<char, Vector2> TilePositions {get; private set;}

    private void Awake() {
        //Setting singleton reference
        if (Instance != null) Destroy(this);
        Instance = this;

        FindLoader();
        this.TilePositions = loader.GetTilePositions();
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
