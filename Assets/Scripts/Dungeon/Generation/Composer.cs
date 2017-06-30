// ReSharper disable MemberCanBePrivate.Global
public class Composer {
	private readonly Dungeon dungeon;

	public Composer(Dungeon d) {
		dungeon = d;
	}

	public Composer begin(int width, int height) {
		dungeon.InitBlockArray(width, height);

		return this;
	}

	public Composer block(int x, int y) {
		dungeon.setBlock(new Position(x, y));

		return this;
	}

	public Composer block(Position p) {
		dungeon.setBlock(p);

		return this;
	}

	public Composer corridor(params Position[] points) {
		if(points.Length > 1) {
			for(var i = 0; i < points.Length - 1; i++) {
				var p0 = points[i];
				var p1 = points[i + 1];

				if(p0.x == p1.x) {
					for(var y = p0.y; y != p1.y; y = p1.y < p0.y ? y - 1 : y + 1) {
						dungeon.setBlock(new Position(p0.x, y));
					}
				} else if(p0.y == p1.y) {
					for(var x = p0.x; x <= p1.x; x = p1.x < p0.x ? x - 1 : x + 1) {
						dungeon.setBlock(new Position(x, p0.y));
					}
				}
			}
		} else if(points.Length == 1) {
			return block(points[0]);
		}

		return this;
	}

	public Composer area(Area a) {
		a.ForEachValidPos(dungeon, delegate(Position pos) {
			dungeon.setBlock(pos);
		});

		return this;
	}

	public Composer area(Position p0, Position p1) {
		return area(Area.fromCoords(p0, p1));
	}

	public Composer area(int x0, int y0, int x1, int y1) {
		return area(Area.fromCoords(x0, y0, x1, y1));
	}

	public Composer player(int x, int y, Direction dir) {
		dungeon.setPlayerSpawn(new Position(x, y));
		dungeon.spawnPlayer(dir);

		return this;
	}

	public Composer player(Position p, Direction dir) {
		return player(p.x, p.y, dir);
	}

	public Composer monster(string id, Position[] positions) {
		foreach(var pos in positions) {
			dungeon.spawnMonster(pos, id);
		}

		return this;
	}

	public Composer monster(string id, Position pos, Direction dir) {
		return monster(id, new [] { pos });
	}

	public Composer chest(Position pos, Direction dir, bool locked, params Item[] content) {
		dungeon.spawnChest(pos, dir, locked, content);

		return this;
	}

	public void finish() {
		dungeon.ForEachPosition(delegate(Position pos) {
			var block = dungeon.getBlock(pos);
			if(block != null) {
				var adj = dungeon.getAdjBlocks(pos);

				foreach(var dir in Direction.All) {
					if(adj[dir.Num] != null) {
						block
							.setPassable(dir)
							.removeTexture(dir);
					} else {
						block
							.setUnpassable(dir)
							.setTexture(0, dir);
					}
				}
			}
		});
	}
}