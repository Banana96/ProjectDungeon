using UnityEngine;

public class MenuButton : MonoBehaviour {
	public QuitGamePanel quitPanel;

	public bool isOpen { get; private set; }

	public void Toggle(bool open) {
		isOpen = open;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, isOpen ? 180 : 0));
		if(quitPanel.isOpen != isOpen) {
			quitPanel.Toggle(isOpen);
		}
	}

	public void Toggle() {
		Toggle(!isOpen);
	}
}