/// <summary>Abstracted class for automated dungeon in-game init.</summary>
public abstract class DungeonBuilder {
	protected delegate void BuildProcess();

	protected readonly Dungeon dungeon;
	protected BuildProcess buildProcess;

	protected DungeonBuilder(Dungeon d) {
		dungeon = d;
	}

	protected void BuildMesh() {
		if(dungeon.transform.Find("_staticMesh") == null) {
			DungeonMeshBuilder.BuildMesh();
		}
	}

	public void Build() {
		buildProcess();
	}
}
