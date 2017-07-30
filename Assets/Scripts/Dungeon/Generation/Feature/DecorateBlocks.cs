public partial class DungeonGenerator {
	private const int DecorationChance = 12;

	private void DecorateBlocks() {
		var tx = dungeon.textures;

		dungeon.ForEachExistingBlock(delegate (Position pos, Block b) {
			var w = b.textures;
			for(var i = 0; i < w.Length; i++) {
				if(w[i] != -1 & rng.Next(100) <= DecorationChance) {
					if(i >= 0 & i < 4 & tx.multipleWalls) {
						if(!b.special[i]) {
							//b.setTexture(tx.rWall(rng), i);
							b.textures[i] = tx.rWall(rng);
						}
					} else if(i == 4 & tx.multipleFloors) {
						//b.setTexture(tx.rFloor(rng), i);
						b.textures[i] = tx.rFloor(rng);
					} else if(i == 5 & tx.multipleCeilings) {
						//b.setTexture(tx.rCeiling(rng), i);
						b.textures[i] = tx.rCeiling(rng);
					}
				}
			}
		});
	}
}
