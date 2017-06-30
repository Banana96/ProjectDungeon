using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class AreaLinker : FeatureGenerator {
	public override bool generate(Dungeon dungeon, Random rng) {
		foreach(var area in dungeon.areas) {
			var availableSides = new List<Direction>();

			if(area.minY != 0) {
				availableSides.Add(Direction.North);
			}

			if(area.maxY != dungeon.Height - 1) {
				availableSides.Add(Direction.South);
			}

			if(area.maxX != dungeon.Width - 1) {
				availableSides.Add(Direction.East);
			}

			if(area.minX != 0) {
				availableSides.Add(Direction.West);
			}

			var side = U.RandArrElem(availableSides.ToArray(), rng);
			var it = area.edgeIterator(side);

			var linked = false;

			do {
				var p = new Position(rng.Next(it[0].x, it[1].x), rng.Next(it[0].y, it[1].y));
				var block = dungeon.getBlock(p);
				var adjBlock = dungeon.getAdjBlock(p, side.GetOpposite());

				if(adjBlock != null) {
					Direction s = side, os = side.GetOpposite();

					Debug.Assert(block != null);

					block
						.setTexture(dungeon.textures.doorOpen, os)
						.setSpecial(os)
						.setPassable(os);

					adjBlock
						.setTexture(dungeon.textures.doorOpen, s)
						.setSpecial(s)
						.setPassable(s);

					linked = true;
				}
			} while(!linked);
		}

		return true;
	}
}