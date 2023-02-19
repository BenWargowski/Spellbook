using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tile plays a little blinking animation during spellwriting when its letter is pressed
/// </summary>
public class TileSpellwrite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Instance.alphabetKeyPressed += OnSpellWrite;
    }

    void OnSpellWrite(char c, bool shift) {
        if (!shift) return;

        GameObject tile = StageLayout.Instance.Tiles[c];
        Tile tileData = null;
        if (!tile.TryGetComponent<Tile>(out tileData)) return;

        Animator animator = null;
        if (!tileData.Sprite.TryGetComponent<Animator>(out animator)) return;

        animator.Play("Blink");
    }
}
