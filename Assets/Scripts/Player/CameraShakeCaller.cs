using UnityEngine;

/// <summary>
/// Small script to be attached onto Earthquake effects prefab to call the camera shake.
/// Does not handle camera shaking itself, see CameraShake.cs for that.
/// </summary>
public class CameraShakeCaller : MonoBehaviour {
	[SerializeField] private float duration;
	[SerializeField] private float magnitude;

	private void Start() {
		Camera camera = Camera.main;
		CameraShake shaker = null;
		if (camera != null && camera.TryGetComponent<CameraShake>(out shaker)) {
			shaker.ScreenShake(this.duration, this.magnitude);
		}
	}
}