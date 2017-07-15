using System;
using UnityEngine;

/// <summary>Camera animation used to indicate player rotation.</summary>
public class RotateCamera : CameraAnimation {
	private const float rotateRate = 4f;

	private float progress;
	private float currentRot;

	private readonly float initRot;
	public readonly float destRot;

	public RotateCamera(Camera cam, bool cw) : base(cam) {
		initRot = Mathf.RoundToInt(camera.transform.eulerAngles.y);
		currentRot = initRot;
		destRot = Mathf.RoundToInt(initRot + (cw ? 90 : -90));
	}

	public override void Update() {
		progress = Mathf.Clamp01(progress + (rotateRate * Time.deltaTime));
		currentRot = Mathf.Lerp(initRot, destRot, progress);
		var yRot = Mathf.Sin(progress * Mathf.PI) * -3;

		camera.transform.rotation = Quaternion.Euler(yRot, currentRot, 0);

		if(Math.Abs(progress - 1) < float.Epsilon) {
			camera.transform.rotation = Quaternion.Euler(0, destRot, 0);
			done = true;
		}
	}
}
