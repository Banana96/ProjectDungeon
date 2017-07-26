using System.Collections.Generic;

public partial class DungeonGenerator {
	private List<Direction> AvailableDirs(Dungeon d, Position from) {
		var dirs = new List<Direction>();

		foreach(var dir in Direction.All) {
			if(d.isPosValid(from.copy().add(dir.UX * 2, dir.UZ * 2))) {
				var pos = from + dir;

				if(d.getBlock(pos) == null) {
					pos += dir;

					if(d.getBlock(pos) == null) {
						dirs.Add(dir);
					}
				}
			}
		}

		return dirs;
	}

	private void GenerateCorridors() {
		var cells = new List<Position>();

		Position first;

		do {
			first = new Position(rng.Next(dungeon.Width), rng.Next(dungeon.Height));
		} while(dungeon.getBlock(first) != null);

		cells.Add(first);
		dungeon.setBlock(first);

		while(cells.Count > 0) {
			var last = cells[rng.Next(cells.Count)];
			var adj = AvailableDirs(dungeon, last);

			if(adj.Count == 0) {
				cells.Remove(last);
			} else {
				var d = U.RandArrElem(adj, rng);
				var n = last + d;

				dungeon.getBlock(last).removeTexture(d).setPassable(d);
				dungeon.setBlock(n).removeTexture(d.Opposite).setPassable(d.Opposite);

				cells.Add(n);
			}
		}
	}
}
