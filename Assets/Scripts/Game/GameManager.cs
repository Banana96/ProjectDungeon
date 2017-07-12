using Campaign;
using UnityEngine;

// ReSharper disable ObjectCreationAsStatement
public class GameManager : MonoBehaviour {
	public PlayMode playMode;

	public void Start() {
		var dungeonObj = new GameObject("Dungeon") {tag = "Dungeon"};
		var dungeon = dungeonObj.AddComponent<Dungeon>();
		DungeonBuilder builder;

		if(playMode == PlayMode.Campaign) {
			builder = new TutorialMission(dungeon);
		} else {
			builder = new DungeonGenerator(dungeon);
		}

		builder.Build();

		new GameObject("PlayerInterface")
			.AddComponent<PlayerInterface>()
			.attachDungeon();

		DestroyImmediate(gameObject);
	}
}