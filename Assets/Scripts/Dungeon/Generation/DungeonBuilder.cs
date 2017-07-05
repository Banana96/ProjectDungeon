/// <summary>Abstracted class for automated dungeon in-game init.</summary>
public abstract class DungeonBuilder {
	protected readonly Dungeon dungeon;

	protected DungeonBuilder(Dungeon d) {
		dungeon = d;

		Work();

		if(dungeon.transform.Find("_staticMesh") == null) {
			DungeonMeshBuilder.BuildMesh();
		}

		dungeon.autoInitPlayer();
	}

	/// <summary>Actual algorithm to generate complete dungeon.</summary>
	protected abstract void Work();
}