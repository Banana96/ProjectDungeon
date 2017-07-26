using Campaign;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public PlayMode playMode;
	public int generatorSeed;

	public void Start() {
		var dungeonObj = new GameObject("Dungeon") { tag = "Dungeon" };
		var dungeon = dungeonObj.AddComponent<Dungeon>();
		DungeonBuilder builder;

		if(playMode == PlayMode.Campaign) {
			builder = new TutorialMission(dungeon);
		} else {
			builder = new DungeonGenerator(dungeon, generatorSeed);
		}

		builder.Build();

		var gen = builder as DungeonGenerator;

		if(gen != null) {
			generatorSeed = gen.seed;
		}

		new GameObject("PlayerInterface")
			.AddComponent<PlayerInterface>()
			.attachDungeon();
	}
}
