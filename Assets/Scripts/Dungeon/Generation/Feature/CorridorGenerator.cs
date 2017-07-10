using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>Generates corridors using growing tree algorithm.</summary>
public class CorridorGenerator : FeatureGenerator {
	public override bool generate(Dungeon dungeon, Random rng) {
		var cells = new Stack<Position>();

		Position first;

		do {
			first = new Position(rng.Next(dungeon.Width), rng.Next(dungeon.Height));
		} while(dungeon.getBlock(first) != null);

		cells.Push(first);
		dungeon.setBlock(first);

		while(cells.Count > 0) {
			var last = cells.Peek();
			var adj = dungeon.getNullAdjBlocks(last);

			if(adj.Count == 0) {
				cells.Pop();
			} else {
				var d = adj.Count > 1 ? U.RandArrElem(adj, rng) : adj[0];
				var n = last.copy().add(d);

				dungeon.getBlock(last).removeTexture(d).setPassable(d);
				dungeon.setBlock(n).removeTexture(d.GetOpposite()).setPassable(d.GetOpposite());

				cells.Push(n);
			}
		}

		Debug.Log("Corridors generated");

		return true;
	}
}