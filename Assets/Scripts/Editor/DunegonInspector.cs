using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dungeon))]
public class DunegonInspector : Editor {
	private Dungeon dng => (Dungeon)target;

	public override void OnInspectorGUI() {
		// Size
		EditorGUILayout.LabelField("Size: ", dng.Width+"x"+dng.Height);

		if(!Application.isPlaying) {
			// Size editor
			if(GUILayout.Button("Resize")) {
				DungeonSizeEditor.OpenWindow();
			}
		}
	}

	public void OnSceneGUI() {
		if(Application.isPlaying) {
			Handles.DrawWireCube(
				new Vector3(dng.Width/2 - 0.5f, 0.5f, dng.Height/2 - 0.5f),
				new Vector3(dng.Width, 1, dng.Height)
			);
		}
	}
}
