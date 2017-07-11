using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Chest : Entity {
	public readonly Inventory content = new Inventory(18);
	public bool isLocked = false;

	private Animator animator => GetComponent<Animator>();

	void Awake() {
		var box = new GameObject("Box");
		box.transform.parent = transform;

		var boxMf = box.AddComponent<MeshFilter>();
		boxMf.sharedMesh = Resources.Load<Mesh>("Models/Chest1Box");

		var boxMr = box.AddComponent<MeshRenderer>();
		boxMr.sharedMaterial = Resources.Load<Material>("Materials/Chest1Box");

		var cover = new GameObject("Cover");
		cover.transform.parent = transform;

		var coverMf = cover.AddComponent<MeshFilter>();
		coverMf.sharedMesh = Resources.Load<Mesh>("Models/Chest1Cover");

		var coverMr = cover.AddComponent<MeshRenderer>();
		coverMr.sharedMaterial = Resources.Load<Material>("Materials/Chest1Cover");
	}

	/// <summary>Play chest unlocking animation.</summary>
	private void animUnlock() {
		animator.Play("Chest_Unlock");
	}

	/// <summary>Play chest opening animation.</summary>
	private void animOpen() {
		animator.Play("Chest_Opening");
	}

	/// <summary>Play chest closing animation.</summary>
	private void animClose() {
		animator.Play("Chest_Closing");
	}

	public override void pushRequest() {
		request = null;
	}
}