using UnityEngine;

public abstract class CameraAnimation {
	protected readonly Camera camera;
	public bool done { get; protected set; }

	protected CameraAnimation(Camera cam) {
		camera = cam;
	}

	public abstract void Update();
}