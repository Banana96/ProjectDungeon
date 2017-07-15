using UnityEditor;
using UnityEngine;

public class DungeonSizeEditor : EditorWindow {
	private Position newSize = new Position();

	public static void OpenWindow() {
		var window = GetWindow<DungeonSizeEditor>();
		var d = Dungeon.Instance;
		window.newSize = new Position(d.Width, d.Height);
	}

	public void OnGUI() {
		var d = Dungeon.Instance;
		EditorGUILayout.LabelField("New dungeon size");

		if(newSize.x < d.Width || newSize.y < d.Height) {
			
		}

		newSize.x = EditorGUILayout.IntSlider("New width",
			newSize.x, Dungeon.MinSize, Dungeon.MaxSize);

		newSize.y = EditorGUILayout.IntSlider("New height",
			newSize.y, Dungeon.MinSize, Dungeon.MaxSize);

		

		if (GUILayout.Button("Resize")) {
			if(newSize.x < d.Width || newSize.y < d.Height) {
				if(EditorUtility.DisplayDialog("Resize", "Given size is smaller than previous. Clipping may occur.", "Resize")) {
					Dungeon.Instance.InitBlockArray(newSize.x, newSize.y, true);
					Close();
				}
			}
		}
	}
}
