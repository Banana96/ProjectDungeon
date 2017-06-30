using UnityEngine;

public class LightBlink : MonoBehaviour {
	[Range(0f, 7.9f)]
	public float MinIntensity = 0.9f;

	[Range(0.1f, 8f)]
	public float MaxIntensity = 1.1f;
	
	[Range(0f, 1f)]
	public float Speed = 0.5f;

	private Light camLight;
	private float phase;

	void Start() {
		camLight = GetComponent<Light>();
		phase = 0;
	}

	void Update() {
		var diff = (MaxIntensity - MinIntensity);

		camLight.intensity = MinIntensity + diff * (Mathf.Sin(phase) * Speed);

		phase += Speed;
	}
}