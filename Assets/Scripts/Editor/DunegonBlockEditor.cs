using UnityEditor;

public class DunegonBlockEditor : EditorWindow {

	[MenuItem("Dungeon/Blocks")]
	public static void OpenWindow() {
		if(Dungeon.Instance != null) {
			GetWindow<DunegonBlockEditor>();
		}
	}

	private void OnGUI() {
	}
}