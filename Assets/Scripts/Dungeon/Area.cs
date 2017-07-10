using System;
using UnityEngine;

public class Area {
	private int x, y, w, h;

	public int minX { get { return x; } }
	public int minY { get { return y; } }

	public Position min {
		get { return new Position(minX, minY); }
	}

	public int maxX { get { return x + w; } }
	public int maxY { get { return y + h; } }

	public Position max {
		get { return new Position(maxX, maxY); }
	}

	private int centerX { get { return x + w / 2; } }
	private int centerY { get { return y + h / 2; } }

	public Position center {
		get { return new Position(centerX, centerY); }
	}

	public void draw(Dungeon d) {
		for(var ix = minX; ix <= maxX; ix++) {
			for(var iy = minY; iy <= maxY; iy++) {
				var p = new Position(ix, iy);

				var b = d.setBlock(p)
					.removeWallTextures()
					.setAllPassable()
					.setAreaBlock();

				if(p.x == minX) {
					b.setTexture(0, Direction.West).setUnpassable(Direction.West);
				} else if(p.x == maxX) {
					b.setTexture(0, Direction.East).setUnpassable(Direction.East);
				}

				if(p.y == minY) {
					b.setTexture(0, Direction.South).setUnpassable(Direction.South);
				} else if(p.y == maxY) {
					b.setTexture(0, Direction.North).setUnpassable(Direction.North);
				}
			}
		}
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

		for(p.x = minX; p.x < maxX; p.x++) {
			for(p.y = minY; p.y < maxY; p.y++) {
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
		return minX >= x & maxX <= x & minY >= y & maxY <= y;
	}

	public bool contains(Position p) {
		return contains(p.x, p.y);
	}

	public bool overlaps(Area other) {
		return (minX < other.maxX) &&
				(maxX > other.minX) &&
				(minY < other.maxY) &&
				(maxY > other.minY);
	}

	public bool touches(Area other) {
		return (minX <= other.maxX) &&
				(maxX >= other.minX) &&
				(minY <= other.maxY) &&
				(maxY >= other.minY);
	}

	public override string ToString() {
		return "[x0:" + minX + ", y0:" + minY + ", x1:" + maxX + ", y1:" + maxY + "]";
	}

	public bool onEdge(int x0, int y0) {
		if(contains(x0, y0)) {
			return x0 == minX || x0 == maxX || y0 == minY || y0 == maxY;
		}

		return false;
	}

	public bool onCorner(int cx, int cy) {
		if(contains(cx, cy)) {
			return (cx == minX && cy == minY) || (cx == maxX && cy == minY) ||
					(cx == minX && cy == maxY) || (cx == maxX && cy == maxY);
		}
		return false;
	}
}