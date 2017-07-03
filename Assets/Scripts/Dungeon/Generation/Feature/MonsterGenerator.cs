using System;

/// <summary>Generates monsters in dungeon.</summary>
public class MonsterGenerator : FeatureGenerator {
	private int mobLimit { get; set; }
	private int mobCount { get; set; }
	private string[] mobTypes { get; set; }

	public MonsterGenerator(int amount, params string[] types) {
		mobLimit = amount;
		mobCount = 0;

		mobTypes = types;
	}

	public override bool generate(Dungeon dungeon, Random rng) {
		for(var i = 0; i <= mobLimit; i++) {
			Position pos;

			do pos = new Position(rng.Next(0, dungeon.Width), rng.Next(0, dungeon.Height)); while(
				dungeon.getBlock(pos) != null || dungeon.isNearSpecialBlock(pos) || dungeon.getEntity(pos) != null);

			dungeon.spawnMonster(pos, U.RandArrElem(mobTypes, rng));
			mobCount++;
		}

		return true;
	}
}