using UnityEngine;
using UnityEngine.UI;

/// <summary>Camera viewport pixelizer.</summary>
[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour {
	[Range(1, 5)] public int pixelation = 2;

	private void Start() {
		var pxFactor = Mathf.Pow(2, -pixelation);
		var width = (int) (Screen.width * pxFactor);
		var height = (int) (Screen.height * pxFactor);

		var texture = new RenderTexture(width, height, 0) {
			name = "PlayerRender",
			autoGenerateMips = false,
			filterMode = FilterMode.Point,
			anisoLevel = 0,
			depth = 32
		};

		var camera = GetComponent<Camera>();
		camera.targetTexture = texture;

		var render = GameObject.Find("RenderCanvas")
					?? Instantiate(Resources.Load<GameObject>("Prefabs/RenderCanvas"));

		var renderImage = render.transform.Find("PlayerRender").GetComponent<RawImage>();
		renderImage.texture = texture;
	}
}