public partial class DungeonGenerator {
	private void RemoveDeadEnds() {
		var removedEnds = 0;

		do {
			removedEnds = 0;

			dungeon.ForEachExistingBlock(delegate (Position p, Block b) {
				if(b.passableCount == 1) {
					var adjectives = dungeon.getAdjBlocks(p);

					foreach(var d in Direction.All) {
						var adj = adjectives[d];
						if(adj != null) {
							adj.passable[d.Opposite] = false;
							adj.textures[d.Opposite] = 0;
						}
					}

					dungeon.removeBlock(p);

					removedEnds++;
				}
			});
		} while(removedEnds > 0);
	}
}
