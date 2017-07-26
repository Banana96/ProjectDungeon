using System;

public partial class DungeonGenerator {
	private int mobLimit { get; set; }
	private int mobCount { get; set; }
	private string[] mobTypes { get; set; }

	private void SpawnMonsters() {
		for(var i = 0; i <= mobLimit; i++) {
			Position pos;

			do
				pos = new Position(rng.Next(0, dungeon.Width), rng.Next(0, dungeon.Height));
			while(
				dungeon.getBlock(pos) != null || dungeon.isNearSpecialBlock(pos) || dungeon.getEntity(pos) != null);

			dungeon.spawnMonster(pos, U.RandArrElem(mobTypes, rng));
			mobCount++;
		}
	}
}
