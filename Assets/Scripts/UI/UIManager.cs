using UnityEngine;

public class UIManager : MonoBehaviour {
	private GameObject ui;

	void Start() {
		if(Application.isMobilePlatform) {
			ui = Instantiate(Resources.Load("Prefabs/TouchUI") as GameObject);
			ui.name = "TouchUI";
		}
	}
}