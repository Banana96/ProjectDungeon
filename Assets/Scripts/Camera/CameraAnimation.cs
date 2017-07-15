using UnityEngine;

/// <summary>Abstracted base class for in-game player camera animation.</summary>
public abstract class CameraAnimation {
	protected readonly Camera camera;
	public bool done { get; protected set; }

	protected CameraAnimation(Camera cam) {
		camera = cam;
	}

	public abstract void Update();
}
