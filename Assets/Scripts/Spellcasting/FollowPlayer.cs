using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    private Transform player;
    
    private void Start() {
        this.player = FindObjectOfType<Player>().transform;
    }

    private void Update() {
        this.transform.position = this.player.transform.position;
    }
}
