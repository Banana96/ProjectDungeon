public partial class DungeonGenerator {
	private const int MaxTries = 32;

	private void SpawnPlayer() {
		var playerSpawn = U.RandArrElem(dungeon.areas, rng);
		var pos = playerSpawn.center;

		dungeon.spawnPlayer(pos, rng.Next(Direction.All.Length));
	}
}