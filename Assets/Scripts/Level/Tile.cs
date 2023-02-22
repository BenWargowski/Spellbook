using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Holds references to Tile Data
/// </summary>
public class Tile : MonoBehaviour {
    [SerializeField] private GameObject sprite;
    public GameObject Sprite => sprite;
    
    [SerializeField] private TextMeshPro text;
    public TextMeshPro Text => text;
}
