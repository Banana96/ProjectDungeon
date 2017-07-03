using Random = System.Random;
using UnityEngine;

public sealed class Direction {
	public static readonly Direction North = new Direction(0, "North");
	public static readonly Direction East = new Direction(1, "East");
	public static readonly Direction South = new Direction(2, "South");
	public static readonly Direction West = new Direction(3, "West");

	public static readonly Direction[] All = {
		North, East, South, West
	};

	public static Direction Random(Random r) {
		return U.RandArrElem(All, r);
	}

	public static Direction FromAngle(float a) {
		while(a < 0) a += 360;
		while(a >= 360) a -= 360;

		if((a >= 315 & a <= 360) | (a >= 0 & a < 45)) return North;
		if(a >= 45 & a < 135) return East;
		if(a >= 135 & a < 225) return South;
		if(a >= 225 & a < 315) return West;

		Debug.LogError(a);

		return null;
	}

	private readonly int num;
	private readonly Vector2 unit;
	private readonly string name;

	private Direction(int nm, string n) {
		num = nm;
		name = n;

		unit = new Vector2(
			Mathf.Sin(num * Mathf.PI / 2),
			Mathf.Cos(num * Mathf.PI / 2)
		);
	}

	public int Num {
		get { return num; }
	}

	public int UX {
		get { return (int) unit.x; }
	}
	public int UZ {
		get { return (int) unit.y; }
	}

	public int Rot {
		get { return num * 90; }
	}

	public string WallName {
		get { return name; }
	}

	public float GetAngle() {
		if(this == North) return 0;
		if(this == East) return 90;
		if(this == South) return 180;
		return 270;
	}

	public Direction GetOpposite() {
		if(this == North) return South;
		if(this == East) return West;
		if(this == South) return North;
		return East;
	}

	public Direction GetNext() {
		if(this == North) return East;
		if(this == East) return South;
		if(this == South) return West;
		return North;
	}

	public Direction GetPrev() {
		if(this == North) return West;
		if(this == East) return North;
		if(this == South) return East;
		return South;
	}
}