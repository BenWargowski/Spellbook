using System;
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
        // truth table
        // lowercase | shift held || caps lock enabled | mode
        // F         | F          || F                 | move
        // F         | T          || F                 | spell
        // T         | F          || T                 | spell
        // T         | T          || T                 | spell
        if (!Char.IsUpper(c) && !shift) return;
        c = Char.ToUpper(c);

        GameObject tile = StageLayout.Instance.Tiles[c];
        tile = tile.transform.GetChild(0).gameObject;

        Animator animator = null;
        if (!tile.TryGetComponent<Animator>(out animator)) return;

        animator.Play("Blink");
    }
}
