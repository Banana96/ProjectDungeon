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

	public int defaultWall {
		get { return wall[0]; }
	}
	public int defaultFloor {
		get { return floor[0]; }
	}
	public int defaultCeiling {
		get { return ceiling[0]; }
	}

	public int doorClosed {
		get { return closedDoor[0]; }
	}
	public int doorOpen {
		get { return openDoor[0]; }
	}

	public bool multipleWalls {
		get { return wall.Length > 1; }
	}
	public bool multipleFloors {
		get { return floor.Length > 1; }
	}
	public bool multipleCeilings {
		get { return ceiling.Length > 1; }
	}

	public int rWall(Random r) {
		return multipleWalls ? wall[r.Next(wall.Length)] : wall[0];
	}

	public int rFloor(Random r) {
		return multipleFloors ? floor[r.Next(floor.Length)] : floor[0];
	}

	public int rCeiling(Random r) {
		return multipleCeilings ? ceiling[r.Next(floor.Length)] : ceiling[0];
	}
}