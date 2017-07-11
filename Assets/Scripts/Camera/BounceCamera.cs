using System;
using UnityEngine;

/// <summary>Camera animation used to indicate player bounce from wall or an action.</summary>
public class BounceCamera : CameraAnimation {
	private const float moveRate = 15f;
	private const float fastMoveRate = 35f;

	private readonly bool fastMove;
	private float progress;
	private Vector3 initPos;
	private Vector3 currentPos;
	private Vector3 destPos;

	public BounceCamera(Camera cam, Direction d, bool fast = false) : base(cam) {
		fastMove = fast;
		initPos = camera.transform.position;
		currentPos = new Vector3(initPos.x, initPos.y, initPos.z);
		destPos = new Vector3(initPos.x, initPos.y, initPos.z) + new Vector3(d.UX * 0.1f, 0, d.UZ * 0.1f);
	}

	public override void Update() {
		progress = Mathf.Clamp(progress + (fastMove ? fastMoveRate : moveRate) * Time.deltaTime, 0, Mathf.PI);

		currentPos.x = Mathf.Lerp(initPos.x, destPos.x, Mathf.Sin(progress));
		currentPos.z = Mathf.Lerp(initPos.z, destPos.z, Mathf.Sin(progress));

		camera.transform.position = currentPos;

		if(Math.Abs(progress - Mathf.PI) <= float.Epsilon) {
			camera.transform.position = initPos;
			done = true;
		}
	}
}