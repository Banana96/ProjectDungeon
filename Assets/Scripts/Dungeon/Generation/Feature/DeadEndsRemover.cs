using System;

/// <summary>Removes corridors, that are dead ends (have only one passable side).</summary>
public class DeadEndsRemover : FeatureGenerator {
	public override bool generate(Dungeon dungeon, Random rng) {
		int removedEnds;

		do {
			removedEnds = 0;

			for(var y = 0; y < dungeon.Height; y++) {
				for(var x = 0; x < dungeon.Width; x++) {
					var p = new Position(x, y);
					var b = dungeon.getBlock(p);

					if(b != null) {
						if(b.passableCount == 1) {
							var adjectives = dungeon.getAdjBlocks(p);

							foreach(var d in Direction.All) {
								if(adjectives[d.Num] != null) {
									adjectives[d.Num]
										.setUnpassable(d.GetOpposite())
										.setTexture(d.GetOpposite());
								}
							}

							dungeon.removeBlock(p);
							removedEnds++;
						}
					}
				}
			}
		} while(removedEnds > 0);

		return true;
	}
}