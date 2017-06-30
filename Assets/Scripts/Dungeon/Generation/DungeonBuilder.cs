public class DungeonBuilder {
	protected readonly Dungeon dungeon;

	protected DungeonBuilder(Dungeon d) {
		dungeon = d;

		Work();

		if(dungeon.transform.Find("_staticMesh") == null) {
			var dmb = new DungeonMeshBuilder(dungeon);
			dmb.BuildMesh();
		}

		dungeon.autoInitPlayer();
	}

	protected virtual void Work() {}
}