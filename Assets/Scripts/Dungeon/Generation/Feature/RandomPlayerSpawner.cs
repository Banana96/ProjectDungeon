using UnityEngine;
using Random = System.Random;

/// <summary>Spawn player in random free position in a dungeon.</summary>
public class RandomPlayerSpawner : FeatureGenerator {
	private const int MaxTries = 32;

	public bool generate(Dungeon dungeon, Random rng) {
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

		Debug.Log("Player spawned");

		return true;
	}
}