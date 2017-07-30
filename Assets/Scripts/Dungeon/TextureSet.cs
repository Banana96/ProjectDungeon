using System;

public class TextureSet {
	public static readonly TextureSet Default = new TextureSet {
		wall = new byte[] {1, 2},
		floor = new byte[] {3, 4},
		ceiling = new byte[] {13},

		openDoor = new byte[] {5, 10},
		closedDoor = new byte[] {6, 8}
	};

	private byte[] wall;
	private byte[] floor;
	private byte[] ceiling;

	private byte[] openDoor;
	private byte[] closedDoor;

	public byte defaultWall => wall[0];

	public byte defaultFloor => floor[0];
	public byte defaultCeiling => ceiling[0];

	public byte doorClosed => closedDoor[0];
	public byte doorOpen => openDoor[0];

	public bool multipleWalls => wall.Length > 1;
	public bool multipleFloors => floor.Length > 1;
	public bool multipleCeilings => ceiling.Length > 1;

	public byte rWall(Random r) {
		return multipleWalls ? wall[r.Next(wall.Length)] : defaultWall;
	}

	public byte rFloor(Random r) {
		return multipleFloors ? floor[r.Next(floor.Length)] : defaultFloor;
	}

	public byte rCeiling(Random r) {
		return multipleCeilings ? ceiling[r.Next(floor.Length)] : defaultCeiling;
	}
}
