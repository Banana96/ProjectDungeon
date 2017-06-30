using UnityEngine;
using Random = UnityEngine.Random;

public class MoveCamera : CameraAnimation {
	private const float moveRate = 2f;
	private const float signRange = 0.03f;

	private float progress;

	public Direction dir { get; private set; }
	private readonly float sign;

	private Vector3 initPos;
	private Vector3 currentPos;
	private Vector3 destPos;
	
	public MoveCamera(Camera cam, Direction d) : base(cam) {
		dir = d;
		sign = Random.Range(-signRange, signRange);
		initPos = camera.transform.position;
		currentPos = new Vector3(initPos.x, initPos.y, initPos.z);
		destPos = new Vector3(initPos.x, initPos.y, initPos.z) + new Vector3(d.UX, 0, d.UZ);
	}

	public override void Update() {
		progress = Mathf.Clamp(progress + (moveRate * Time.deltaTime), 0, 1);

		var vBobbing = Mathf.Abs(Mathf.Sin(progress * Mathf.PI * 2) * 0.05f);
		var hBobbing = Mathf.Sin(progress * Mathf.PI * 2) * sign;

		currentPos.x = Mathf.Lerp(initPos.x, destPos.x, progress);
		currentPos.y = initPos.y + vBobbing;
		currentPos.z = Mathf.Lerp(initPos.z, destPos.z, progress);

		if(dir == Direction.North || dir == Direction.South) {
			currentPos.x += hBobbing;
		} else {
			currentPos.z += hBobbing;
		}

		camera.transform.position = currentPos;

		if(Mathf.Abs(progress - 1) < float.Epsilon) {
			camera.transform.position = destPos;
			done = true;
		}
	}
}