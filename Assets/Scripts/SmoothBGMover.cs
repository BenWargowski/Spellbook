using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Smoothly moves the background ever so slightly when the player moves,
/// for a parallax-like effect.
/// Intended to be placed on the background object.
/// </summary>
public class SmoothBGMover : MonoBehaviour {
    
    //README: TODO:
    //FOR THE SAKE OF TIME, THIS SCRIPT ASSUMES THAT BOTH THE PLAYER AND CAMERA ARE CENTERED AROUND (0,0)!

    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Options")]
    [SerializeField] private bool inversed;
    [SerializeField] private float scale;
    [SerializeField] private float speed;

    private bool Active {get; set;}
    private Vector2 velocity;

    private void Awake() {
        this.velocity = Vector2.zero;
    }

    private IEnumerator Start() {
        //wait for the player to spawn
        MovementManager movementManager;
        if (this.target.TryGetComponent<MovementManager>(out movementManager)) {
            while (!movementManager.Active) {
                yield return null;
            }
        }

        //Instantly snap to desired position upon start
        this.transform.localPosition = GetDesiredPosition();
       if (target != null) this.Active = true;
       yield break;
    }

    private void LateUpdate() {
        if (!this.Active) return;

        Vector2 smoothedPosition = Vector2.SmoothDamp(this.transform.localPosition, GetDesiredPosition(), ref this.velocity, this.speed);

        this.transform.localPosition = smoothedPosition;
    }

    private Vector2 GetDesiredPosition() {
        return (this.inversed ? -1 : 1) * target.localPosition * this.scale;
    }
}
