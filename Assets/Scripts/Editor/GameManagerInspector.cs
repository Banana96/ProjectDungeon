using UnityEditor;
using GL = UnityEngine.GUILayout;
using EGL = UnityEditor.EditorGUILayout;
using System;

[CustomEditor(typeof(GameManager))]
public class GameManagerInspector : Editor {
	private GameManager gm => (GameManager)target;
	private bool play => EditorApplication.isPlaying;

	public override void OnInspectorGUI() {
		if(play) {
			EGL.LabelField("Play mode", gm.playMode.ToString());
			EGL.LabelField("Gen. seed", gm.generatorSeed.ToString());
		} else {
			gm.playMode = (PlayMode)EGL.EnumPopup("Play mode", gm.playMode);

			if(gm.playMode == PlayMode.RandomDungeon) {
				gm.generatorSeed = EGL.DelayedIntField("Gen. seed", gm.generatorSeed);

				if(GL.Button("Random seed")) {
					gm.generatorSeed = new Random().Next();
				}
			}
		}
	}
}