using System;
using UnityEngine;

public class Area {
	private int x, y, w, h;

	public int minX => x;
	public int minY => y;

	public Position min => new Position(minX, minY);

	public int maxX => x + w;
	public int maxY => y + h;

	public Position max => new Position(maxX, maxY);

	private int centerX => x + w / 2;
	private int centerY => y + h / 2;

	public Position center => new Position(centerX, centerY);

	public void draw(Dungeon d) {
		ForEachValidPos(d, delegate(Position pos) {
			var b = d.setBlock(pos)
					.removeWallTextures()
					.setAllPassable()
					.setAreaBlock();

			if(pos.x == minX) {
				b.setTexture(0, Direction.West).setUnpassable(Direction.West);
			} else if(pos.x == maxX) {
				b.setTexture(0, Direction.East).setUnpassable(Direction.East);
			}

			if(pos.y == minY) {
				b.setTexture(0, Direction.South).setUnpassable(Direction.South);
			} else if(pos.y == maxY) {
				b.setTexture(0, Direction.North).setUnpassable(Direction.North);
			}
		});
	}

	public static Area fromCoords(int x0, int y0, int x1, int y1) {
		return new Area(x0, y0, x1 - x0, y1 - y0);
	}

	public static Area fromCoords(Position p0, Position p1) {
		return fromCoords(p0.x, p0.y, p1.x, p1.y);
	}

	public Area(int x, int y, int w, int h) {
		this.x = x;
		this.y = y;
		this.w = w;
		this.h = h;
	}

	public void ForEachValidPos(Dungeon dungeon, Action<Position> action) {
		var p = min;

		for(p.x = minX; p.x <= maxX; p.x++) {
			for(p.y = minY; p.y <= maxY; p.y++) {
				if(dungeon.isPosValid(p)) {
					action.Invoke(p);
				}
			}
		}
	}

	public Area setPosition(int x0, int y0) {
		x = x0;
		y = y0;
		return this;
	}

	public Area setPosition(Position p) {
		x = p.x;
		y = p.y;
		return this;
	}

	public Area setSize(int sw, int sh) {
		w = sw;
		h = sh;
		return this;
	}

	private bool contains(int x, int y) {
		return minX >= x && maxX <= x
			&& minY >= y && maxY <= y;
	}

	public bool contains(Position p) {
		return contains(p.x, p.y);
	}

	public bool overlaps(Area other) {
		return (minX <= other.maxX) && (maxX >= other.minX)
			&& (minY <= other.maxY) && (maxY >= other.minY);
	}

	public bool touches(Area other) {
		return overlaps(new Area(other.x - 1, other.y - 1, other.w + 2, other.h + 2));
	}

	public override string ToString() {
		return $"Area(p0:[{minX},{minY}], p1:[{maxX},{maxY}])";
	}
}
