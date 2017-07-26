public partial class DungeonGenerator {
	private const int DecorationChance = 12;

	private void DecorateBlocks() {
		var tx = dungeon.textures;

		for(var y = 0; y < dungeon.Height; y++) {
			for(var x = 0; x < dungeon.Width; x++) {
				var b = dungeon.getBlock(x, y);
				if(b != null) {
					var w = b.getTextures();
					for(var i = 0; i < w.Length; i++) {
						if(w[i] != -1 & rng.Next(100) <= DecorationChance) {
							if(i >= 0 & i < 4 & tx.multipleWalls) {
								if(!b.isSpecial(i)) {
									b.setTexture(tx.rWall(rng), i);
								}
							} else if(i == 4 & tx.multipleFloors) {
								b.setTexture(tx.rFloor(rng), i);
							} else if(i == 5 & tx.multipleCeilings) {
								b.setTexture(tx.rCeiling(rng), i);
							}
						}
					}
				}
			}
		}
	}
}
