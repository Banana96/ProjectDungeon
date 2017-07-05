/// <summary>Represents a single block cell in dungeon.</summary>
public class Block {
	/// <summary>Texture IDs (0 is default, -1 is none).</summary>
	private readonly int[] textures = {0, 0, 0, 0, 0, 0};

	/// <summary>Is side of block special.</summary>
	private readonly bool[] special = {false, false, false, false};

	/// <summary>Is side of block passable.</summary>
	private readonly bool[] passable = {false, false, false, false};

	/// <summary>Count textured walls of this block.</summary>
	private int wallCount {
		get {
			var count = 0;

			foreach(var t in textures) {
				if(t > -1) {
					count++;
				}
			}

			return count;
		}
	}

	/// <summary>Count passable sides of this block.</summary>
	public int passableCount {
		get {
			var count = 0;

			foreach(var t in passable) {
				if(t) {
					count++;
				}
			}

			return count;
		}
	}

	/// <summary>Count textured sides of this block.</summary>
	public int textureCount {
		get {
			var count = wallCount;
			if(hasCeilingTexture()) count++;
			if(hasFloorTexture()) count++;

			return count;
		}
	}

	/// <summary>Get ID of texture on the specified side of block.</summary>
	private int getTexture(int wall) {
		if(wall <= 0 & wall > 6) {
			return textures[wall];
		}
		return -1;
	}

	/// <summary>Get ID of texture on the specified wall.</summary>
	private int getTexture(Direction dir) {
		return getTexture(dir.Num);
	}

	/// <summary>Get all texture IDs.</summary>
	public int[] getTextures() {
		return textures;
	}

	/// <summary>Is side textured</summary>
	public bool hasTexture(int wall) {
		return getTexture(wall) != -1;
	}

	/// <summary>Is wall textured</summary>
	public bool hasTexture(Direction dir) {
		return getTexture(dir.Num) != -1;
	}

	/// <summary>Is texture ID on wall equal to 0.</summary>
	public bool hasDefaultTexture(Direction dir) {
		return getTexture(dir) == 0;
	}

	/// <summary>Set side texture.</summary>
	public Block setTexture(int textureId, int wall) {
		textures[wall] = textureId;
		return this;
	}

	/// <summary>Set wall textures.</summary>
	public Block setTexture(int textureId, params Direction[] dirs) {
		foreach(var d in dirs) {
			textures[d.Num] = textureId;
		}

		return this;
	}

	/// <summary>Set default texture.</summary>
	public Block setTexture(params Direction[] dirs) {
		return setTexture(0, dirs);
	}

	/// <summary>Set all walls texture</summary>
	public Block setTextures(int textureId = 0) {
		return setTexture(textureId, Direction.All);
	}

	public Block removeTexture(params Direction[] dirs) {
		return setTexture(-1, dirs);
	}

	public Block removeWallTextures() {
		return removeTexture(Direction.All);
	}

	public int getCeilingTexture() {
		return getTexture(5);
	}

	public bool hasCeilingTexture() {
		return getCeilingTexture() != -1;
	}

	public bool hasDefaultCeilingTexture() {
		return getCeilingTexture() == 0;
	}

	public Block setCeilingTexture(int textureId = 0) {
		textures[5] = textureId;
		return this;
	}

	public Block removeCeilingTexture() {
		return setCeilingTexture(-1);
	}

	public int getFloorTexture() {
		return textures[4];
	}

	public bool hasFloorTexture() {
		return getFloorTexture() != -1;
	}

	public bool hasDefaultFloorTexture() {
		return getFloorTexture() == 0;
	}

	public Block setFloorTexture(int textureId = 0) {
		textures[4] = textureId;
		return this;
	}

	public Block removeFloorTexture() {
		return setFloorTexture(-1);
	}

	public bool isSpecial(int i) {
		return special[i];
	}

	public bool isSpecial(Direction d) {
		return isSpecial(d.Num);
	}

	public Block setSpecial(bool s, params Direction[] dirs) {
		foreach(var d in dirs) {
			special[d.Num] = s;
		}
		return this;
	}

	public Block setSpecial(params Direction[] dirs) {
		return setSpecial(true, dirs);
	}

	public bool isPassable(int i) {
		return passable[i];
	}

	public bool isPassable(Direction dir) {
		return isPassable(dir.Num);
	}

	public Block setPassable(params int[] dirs) {
		foreach(var d in dirs) {
			passable[d] = true;
		}
		return this;
	}

	public Block setPassable(params Direction[] dirs) {
		foreach(var d in dirs) {
			passable[d.Num] = true;
		}
		return this;
	}

	public Block setAllPassable() {
		return setPassable(Direction.All);
	}

	public Block setUnpassable(params int[] dirs) {
		foreach(var d in dirs) {
			passable[d] = false;
		}
		return this;
	}

	public Block setUnpassable(params Direction[] dirs) {
		foreach(var d in dirs) {
			passable[d.Num] = false;
		}
		return this;
	}

	public Block setAllUnpassable() {
		return setUnpassable(Direction.All);
	}
}