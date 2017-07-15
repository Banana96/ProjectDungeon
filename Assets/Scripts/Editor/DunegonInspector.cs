using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dungeon))]
public class DunegonInspector : Editor {
	public override void OnInspectorGUI() {
		var dungeon = (Dungeon) target;
		
		// Size
		EditorGUILayout.LabelField("Size: ", dungeon.Width+"x"+dungeon.Height);
		
		// Size editor
		if(GUILayout.Button("Resize")) {
			DungeonSizeEditor.OpenWindow();
		}

		// Block editor
		if(GUILayout.Button("Edit blocks")) {
			DunegonBlockEditor.OpenWindow();
		}
	}
}
