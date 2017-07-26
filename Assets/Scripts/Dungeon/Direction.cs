using Random = System.Random;
using UnityEngine;

public sealed class Direction {
	public static readonly Direction North	= new Direction(0, "North");
	public static readonly Direction East	= new Direction(1, "East");
	public static readonly Direction South	= new Direction(2, "South");
	public static readonly Direction West	= new Direction(3, "West");

	public static readonly Direction[] All = {
		North, East, South, West
	};

	public static Direction Random(Random r) {
		return U.RandArrElem(All, r);
	}

	public static Direction FromAngle(float angle) {
		int a = ((int)angle % 360 + 360) % 360;

		if((a >= 315 & a <= 360) | (a >= 0 & a < 45)) return North;
		if(a >= 45 & a < 135) return East;
		if(a >= 135 & a < 225) return South;
		if(a >= 225 & a < 315) return West;

		Debug.LogError(a);

		return null;
	}

	public static implicit operator int(Direction dir) => dir.num;
	public static implicit operator Direction(int n) => All[(n % 4 + 4) % 4];

	private readonly byte num;
	private readonly Vector2 unit;
	private readonly string name;

	private Direction(byte nm, string n) {
		num = nm;
		name = n;

		unit = new Vector2(
			Mathf.Sin(num * Mathf.PI / 2),
			Mathf.Cos(num * Mathf.PI / 2)
		);
	}

	public int Num => num;
	public int UX => (int) unit.x;
	public int UZ => (int) unit.y;
	public int Rot => num * 90;
	public string WallName => name;

	public float Angle => num * 90;
	public Direction Opposite => this + 2;
	public Direction Next => this + 1;
	public Direction Prev => this - 1;
}
