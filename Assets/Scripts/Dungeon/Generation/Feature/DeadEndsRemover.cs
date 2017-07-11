using UnityEngine;
using Random = System.Random;

/// <summary>Removes corridors, that are dead ends (have only one passable side).</summary>
public class DeadEndsRemover : FeatureGenerator {
	public bool generate(Dungeon dungeon, Random rng) {
		var max = rng.Next(dungeon.Width * dungeon.Height);
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
					removedEnds++;
				}
			});
		} while(removedEnds > 0 || removedEnds == max);

		Debug.Log("Removed dead ends");

		return true;
	}
}