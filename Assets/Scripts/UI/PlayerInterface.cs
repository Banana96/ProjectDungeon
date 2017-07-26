using System;
using UnityEngine;

public class PlayerInterface : MonoBehaviour {
	private Dungeon dng => Dungeon.Instance;
	private Player player;

	private Camera cam;
	private CameraAnimation camAnim;

	private bool animationFinished => camAnim == null;

	public Response requestResponse;

	public void attachDungeon() {
		if(!dng) {
			Debug.LogError(new ArgumentException("Dungeon can't be null"));
			enabled = false;
			return;
		}

		player = dng.getPlayer();

		if(player == null) {
			Debug.LogError("Player not found");
			enabled = false;
			return;
		}

		var cameraObject = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerCamera"));
		cam = cameraObject.GetComponent<Camera>();
		cam.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
		cam.transform.rotation = Quaternion.Euler(0, player.facing.Angle, 0);
	}

	private void Move(Direction dir) {
		if(animationFinished) {
			var availableRequests = player.getAvailableActions(dir);

			if(availableRequests.Length > 0) {
				player.recieveRequest(availableRequests[0]);

				if(player.request is Move) {
					camAnim = new MoveCamera(cam, dir);
				} else {
					camAnim = new BounceCamera(cam, dir);
				}
			}
		}
	}

	private void Rotate(bool clockwise) {
		if(animationFinished) {
			var rotate = new RotateCamera(cam, clockwise);
			requestResponse = Response.Allow;
			player.facing = Direction.FromAngle(rotate.destRot);
			camAnim = rotate;
		}
	}

	private Direction moveDir;

	private void HandleInput() {
		if(Application.isEditor) {
			if(Input.GetKey(KeyCode.Q)) {
				Rotate(false);
			} else if(Input.GetKey(KeyCode.W)) {
				Move(player.facing);
			} else if(Input.GetKey(KeyCode.E)) {
				Rotate(true);
			} else if(Input.GetKey(KeyCode.D)) {
				Move(player.facing.Next);
			} else if(Input.GetKey(KeyCode.S)) {
				Move(player.facing.Opposite);
			} else if(Input.GetKey(KeyCode.A)) {
				Move(player.facing.Prev);
			} else if(Input.GetKey(KeyCode.Escape)) {
				Application.Quit();
			}
		}

		if(Input.touchCount > 0) {
			var t = Input.GetTouch(0);

			if(t.phase == TouchPhase.Moved && t.deltaPosition.magnitude > 10) {
				var xSign = t.deltaPosition.x / Mathf.Abs(t.deltaPosition.x);
				var angle = Vector2.Angle(Vector2.up, t.deltaPosition) * xSign;

				moveDir = Direction.FromAngle(angle);
			}

			if(moveDir != null) {
				if(moveDir == Direction.North) {
					Move(player.facing);
				} else if(moveDir == Direction.East) {
					Rotate(true);
				} else if(moveDir == Direction.South) {
					Move(player.facing.Opposite);
				} else if(moveDir == Direction.West) {
					Rotate(false);
				}

				moveDir = null;
			}
		}
	}

	void Update() {
		HandleInput();

		if(player.hasRequest) {
			dng.requestUpdate(this);
		}

		if(!animationFinished && requestResponse != Response.Empty) {
			if(requestResponse == Response.Allow) {
				if(camAnim.done) {
					camAnim = null;
					requestResponse = Response.Empty;
				} else {
					camAnim.Update();
				}
			} else if(requestResponse == Response.Deny) {
				var mc = camAnim as MoveCamera;
				if(mc != null) {
					var dir = mc.dir;
					camAnim = new BounceCamera(cam, dir);
					requestResponse = Response.Allow;
				} else {
					camAnim = null;
					requestResponse = Response.Empty;
				}
			}
		}
	}
}
