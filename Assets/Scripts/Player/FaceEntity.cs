using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes one entity always face another
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FaceEntity : MonoBehaviour {

    [SerializeField] private bool active;
    [SerializeField] private int overrideFlipped; //hacky workaround for Animator to work properly
    [SerializeField] private Transform target;
    [SerializeField] private bool flipped;

    private SpriteRenderer rend;

    private void Awake() {
        rend = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update() {
        if (!active) {
            if (overrideFlipped != 0) rend.flipX = overrideFlipped == 1;
            return;
        }

        if (target.position.x < this.transform.position.x) {
            rend.flipX = flipped;
        }
        else {
            rend.flipX = !flipped;
        }
    }
}
