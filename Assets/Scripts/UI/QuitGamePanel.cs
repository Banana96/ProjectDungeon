using UnityEngine;
using System.Collections;

public class QuitGamePanel : MonoBehaviour {
	public MenuButton menuButton;

	public bool isOpen { get { return gameObject.activeInHierarchy; } }

	void Awake() {
		gameObject.SetActive(false);
	}

	public void Toggle(bool open) {
		gameObject.SetActive(open);
		if(menuButton.isOpen != open) {
			menuButton.Toggle(open);
		}
	}

	public void Toggle() {
		Toggle(!isOpen);
	}

	public void Quit() {
		Debug.Log("User called Quit()");
		Application.Quit();
	}

	public void Cancel() {
		Toggle(false);
	}
}