using System;

public class TextureSet {
	public static readonly TextureSet Default = new TextureSet {
		wall = new[] {1, 2},
		floor = new[] {3, 4},
		ceiling = new[] {13},

		openDoor = new[] {5, 10},
		closedDoor = new[] {6, 8}
	};

	private int[] wall;
	private int[] floor;
	private int[] ceiling;

	private int[] openDoor;
	private int[] closedDoor;

	public int defaultWall => wall[0];

	public int defaultFloor => floor[0];
	public int defaultCeiling => ceiling[0];

	public int doorClosed => closedDoor[0];
	public int doorOpen => openDoor[0];

	public bool multipleWalls => wall.Length > 1;
	public bool multipleFloors => floor.Length > 1;
	public bool multipleCeilings => ceiling.Length > 1;

	public int rWall(Random r) {
		return multipleWalls ? wall[r.Next(wall.Length)] : defaultWall;
	}

	public int rFloor(Random r) {
		return multipleFloors ? floor[r.Next(floor.Length)] : defaultFloor;
	}

	public int rCeiling(Random r) {
		return multipleCeilings ? ceiling[r.Next(floor.Length)] : defaultCeiling;
	}
}
