using UnityEngine;

/// <summary>Used to make attached Light component to flicker.</summary>
public class LightFlicker : MonoBehaviour {
	[Range(45, 135)] public int MinimumAngle = 75;

	[Range(50, 140)] public int MaximumAngle = 80;

	[Range(1f, 2f)] public float ColorFactor = 1.25f;

	private Light camLight;

	void Start() {
		camLight = GetComponent<Light>();
	}

	void Update() {
		var spotDiff = Mathf.Sin(Random.Range(-180, 180) * Mathf.Deg2Rad);
		camLight.spotAngle = Mathf.Clamp(camLight.spotAngle + spotDiff, MinimumAngle, MaximumAngle);

		var orange = new Color(1, 0.5f, 0, 1);
		camLight.color = Color.Lerp(orange, Color.white, camLight.spotAngle / (MaximumAngle * ColorFactor));
	}
}