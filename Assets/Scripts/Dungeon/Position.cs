public class Position {
	public int x, y;

	public Position(int x = 0, int y = 0) {
		this.x = x;
		this.y = y;
	}

	public static bool Compare(Position p1, Position p2) {
		if(p1 != null && p2 != null) {
			return p1.x == p2.x && p1.y == p2.y;
		}

		return p1 == p2;
	}

	public Position copy() {
		return new Position(x, y);
	}

	public Position add(int x0, int y0) {
		x += x0; y += y0;
		return this;
	}

	public Position add(Direction dir) {
		x += dir.UX; y += dir.UZ;
		return this;
	}

	public static Position operator +(Position l, Position r) {
		return new Position(l.x + r.x, l.y + r.y);
	}

	public static Position operator +(Position l, Direction d) {
		return new Position(l.x + d.UX, l.y + d.UZ);
	}

	public override string ToString() {
		return "["+x+", "+y+"]";
	}

	public override bool Equals(object obj) {
		if(obj != null && obj.GetType() == typeof(Position)) {
			var pos = obj as Position;
			return pos != null && x == pos.x && y == pos.y;
		}

		return false;
	}

	public override int GetHashCode() {
		return base.GetHashCode();
	}
}