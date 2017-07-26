public partial class DungeonGenerator {
	private const int MaxTries = 32;

	private void SpawnPlayer() {
		var tries = 0;

		do {
			var pos = new Position(
				rng.Next(dungeon.Width - 1),
				rng.Next(dungeon.Height - 1)
			);

			if(dungeon.getBlock(pos) != null) {
				dungeon.spawnPlayer(pos, null);
				break;
			}

			tries++;
		} while(tries < MaxTries);
	}
}