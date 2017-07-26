using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MovementButton : MonoBehaviour {
	public enum MovementType {
		RotateLeft, Forward, RotateRight,
		Left, Back, Right
	}

	public void OnPress() {
		
	}
}
