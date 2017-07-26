// ReSharper disable MemberCanBePrivate.Global

/// <summary>
/// Composer allows to quickly BuildProcess whole <c>Dungeon</c> with command chain.
/// </summary>
public class Composer {
	private readonly Dungeon dungeon;

	public Composer(Dungeon d) {
		dungeon = d;
	}

	/// <summary>Begin building and initialize dungeon size. This should
	/// be called once before any other <c>Composer</c> command.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer begin(int width, int height) {
		dungeon.InitBlockArray(width, height);

		return this;
	}

	/// <summary>Place single block in given coordinates.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer block(int x, int y) {
		dungeon.setBlock(new Position(x, y));

		return this;
	}

	/// <summary>Place single block in given position.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer block(Position p) {
		dungeon.setBlock(p);

		return this;
	}

	/// <summary>Place corridor point-to-point in given order.</summary>
	/// <remarks>Places single block if given only one coordinate.</remarks>
	/// <param name="points">Points of corridor in order</param>
	/// <returns>This <c>Composer</c>.</returns>
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

	/// <summary>Create empty area.</summary>
	/// <param name="a">Area to be created.</param>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer area(Area a) {
		a.draw(dungeon);

		return this;
	}

	/// <summary>Create empty area using two diagonal positions.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer area(Position p0, Position p1) {
		return area(Area.fromCoords(p0, p1));
	}

	/// <summary>Create empty area using two diagonal coordinates.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer area(int x0, int y0, int x1, int y1) {
		return area(Area.fromCoords(x0, y0, x1, y1));
	}

	/// <summary>Spawn new player instance.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer player(int x, int y, Direction dir) {
		dungeon.spawnPlayer(new Position(x, y), dir);

		return this;
	}

	/// <summary>Spawn new player instance.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer player(Position p, Direction dir) {
		return player(p.x, p.y, dir);
	}

	/// <summary>Spawn multiple new instances of monster.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer monster(string id, Position[] positions) {
		foreach(var pos in positions) {
			dungeon.spawnMonster(pos, id);
		}

		return this;
	}

	/// <summary>Spawn new monster instance.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer monster(string id, Position pos) {
		return monster(id, new[] {pos});
	}

	/// <summary>Spawn new chest instance.</summary>
	/// <returns>This <c>Composer</c>.</returns>
	public Composer chest(Position pos, Direction dir, bool locked, params Item[] content) {
		dungeon.spawnChest(pos, dir, locked, content);

		return this;
	}

	/// <summary>Finalize dungeon composing. Joins all adjective blocks.</summary>
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
