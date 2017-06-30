using Random = System.Random;

public class PlayerSpawner : FeatureGenerator {
	private const int MaxTries = 32;

	public override bool generate(Dungeon dungeon, Random rng) {
		var tries = 0;

		do {
			var pos = new Position(
				rng.Next(dungeon.Width - 1),
				rng.Next(dungeon.Height - 1)
			);

			if(dungeon.getBlock(pos) != null) {
				dungeon.setPlayerSpawn(pos);
				break;
			}

			tries++;
		} while(tries < MaxTries);

		return true;
	}
}