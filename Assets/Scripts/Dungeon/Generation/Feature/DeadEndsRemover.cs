using UnityEngine;
using Random = System.Random;

/// <summary>Removes corridors, that are dead ends (have only one passable side).</summary>
public class DeadEndsRemover : FeatureGenerator {
	public bool generate(Dungeon dungeon, Random rng) {
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

			dungeon.ForEachExistingBlock(delegate(Position p, Block b) {
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

					removedTotal++;
					removedEnds++;
				}
			});
		} while(removedEnds > 0 && removedTotal <= max);

		return true;
	}
}
