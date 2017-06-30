using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	private Animator anim { get { return GetComponent<Animator>(); } }

	public void NewGame() {
		anim.Play("OnNewGame");
	}

	public void OnFinishNewGame() {
		SceneManager.LoadScene("game");
	}

	public void Settings() {}

	public void Quit() {
		Application.Quit();
	}
}