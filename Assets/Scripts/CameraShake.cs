using UnityEngine;

// This script should be placed on the Camera (or its parent, etc.)
public class CameraShake : MonoBehaviour {

    /// <summary>
    /// Remaining time in seconds the camera should continue shaking for
    /// </summary>
    private float shakeDuration;
    
    /// <summary>
    /// How violently the camera should shake
    /// </summary>
    private float magnitude;


    /// <summary>
    /// This is manually set by the script at runtime. Stores original position of camera to return to.
    /// Please note this means if the camera moves at all during runtime, it will return to this original position after shaking.
    /// Thus, if this script is attached to the camera, movement should be done on a parent "anchor" object (or vice versa).
    /// </summary>
    private Vector3 originalPosition;

    /// <summary>
    /// Shake the camera for this duration.
    /// If camera is already shaking, it will continue to do so
    /// for its current duration, or this new one, whichever is longer.
    /// </summary>
    /// <param name="duration">duration in seconds</param>
    public void ScreenShake(float duration, float magnitude) {
        this.shakeDuration = Mathf.Max(this.shakeDuration, duration);
        this.magnitude = Mathf.Max(this.magnitude, magnitude);
    }

    private void Awake() {
        this.shakeDuration = 0.0f;
        this.magnitude = 0.0f;
    }

    private void Start() {
        this.originalPosition = transform.localPosition;
    }

    private void Update() {
        //if shake has stopped, make sure magnitude and positions are reset
        if (this.shakeDuration <= 0.0f) {
            this.magnitude = 0.0f;
            this.transform.localPosition = this.originalPosition;
            return;
        }

        //Camera shake behaviour
        float x = Random.Range(-1.0f, 1.0f) * this.magnitude;
        float y = Random.Range(-1.0f, 1.0f) * this.magnitude;

        transform.localPosition = new Vector3(x, y, this.originalPosition.z);
        this.shakeDuration -= Time.deltaTime;
    }
}
