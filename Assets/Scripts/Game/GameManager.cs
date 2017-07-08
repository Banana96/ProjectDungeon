using Campaign;
using UnityEngine;

// ReSharper disable ObjectCreationAsStatement
public class GameManager : MonoBehaviour {
	public PlayMode playMode;

	public void Start() {
		var dungeonObj = new GameObject("Dungeon") {tag = "Dungeon"};
		var dungeon = dungeonObj.AddComponent<Dungeon>();

		if(playMode == PlayMode.Campaign) {
			new TutorialMission(dungeon);
		} else {
			new DungeonGenerator(dungeon);
		}

		new GameObject("PlayerInterface")
			.AddComponent<PlayerInterface>()
			.attachDungeon();

		DestroyImmediate(gameObject);
	}
}