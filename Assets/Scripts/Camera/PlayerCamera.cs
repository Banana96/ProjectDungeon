using UnityEngine;
using UnityEngine.UI;
using Size = Position;

/// <summary>Camera viewport pixelizer.</summary>
[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour {
	private const int LandscapeFOV = 70;
	private const int PortraitFOV = 90;

	[Range(1, 5)] public int pixelationLevel = 2;

	private Size resolution;
	private DeviceOrientation orientation;

	private Size currentRes => new Size(Screen.width, Screen.height);
	private DeviceOrientation currentOrient => Input.deviceOrientation;

	private GameObject renderCanvasObject { get; set; }

	private bool isLandscape =>
		orientation == DeviceOrientation.LandscapeLeft
		|| orientation == DeviceOrientation.LandscapeRight
		|| resolution.x > resolution.y;

	private void Start() {
		resolution = new Size(Screen.width, Screen.height);
		orientation = Input.deviceOrientation;

		ReloadCanvas();
	}

	private void FixedUpdate() {
		if(orientation != currentOrient || !Size.Compare(resolution, currentRes)) {
			orientation = currentOrient;
			resolution = currentRes;

			ReloadCanvas();
		}
	}

	private void ReloadCanvas() {
		var pxFactor = Mathf.Pow(2, -pixelationLevel);
		var width = (int)(resolution.x * pxFactor);
		var height = (int)(resolution.y * pxFactor);

		var texture = new RenderTexture(width, height, 0) {
			name = "PlayerRender",
			autoGenerateMips = false,
			filterMode = FilterMode.Point,
			anisoLevel = 0,
			depth = 32
		};

		var cam = GetComponent<Camera>();
		cam.targetTexture = texture;

		cam.fieldOfView = isLandscape ? LandscapeFOV : PortraitFOV;

		if(renderCanvasObject == null) {
			renderCanvasObject = Instantiate(Resources.Load<GameObject>("Prefabs/RenderCanvas"));
		}

		var renderImage = renderCanvasObject.transform.Find("PlayerRender").GetComponent<RawImage>();
		renderImage.texture = texture;
	}
}