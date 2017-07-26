public partial class DungeonGenerator {
	private void RemoveDeadEnds() {
		var corridorBlocks = 0;

		dungeon.ForEachExistingBlock(delegate(Position pos, Block b) {
			if(!b.areaBlock) {
				corridorBlocks++;
			}
		});

		var max = corridorBlocks * 3 / 4;

		var removedTotal = 0;
		var removedEnds = 0;

		do {
			removedEnds = 0;

			dungeon.ForEachExistingBlock(delegate (Position p, Block b) {
				if(b.passableCount == 1) {
					var adjectives = dungeon.getAdjBlocks(p);

					foreach(var d in Direction.All) {
						if(adjectives[d.Num] != null) {
							adjectives[d.Num]
								.setUnpassable(d.Opposite)
								.setTexture(d.Opposite);
						}
					}

					dungeon.removeBlock(p);

					removedTotal++;
					removedEnds++;
				}
			});
		} while(removedEnds > 0 && removedTotal <= max);
	}
}
