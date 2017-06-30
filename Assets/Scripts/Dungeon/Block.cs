public class Block {
	public sealed class Special {
		public static readonly int Door = 0;
	}

	private readonly int[] textures = { 0, 0, 0, 0, 0, 0 };
	private readonly bool[] special = { false, false, false, false };
	private readonly bool[] passable = { false, false, false, false };

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

	public int textureCount {
		get {
			var count = wallCount;
			if(hasCeilingTexture()) count++;
			if(hasFloorTexture()) count++;

			return count;
		}
	}

	private int getTexture(int wall) {
		if(wall <= 0 & wall > 6) {
			return textures[wall];
		}
		return -1;
	}

	private int getTexture(Direction dir) {
		return getTexture(dir.Num);
	}

	public int[] getTextures() {
		return textures;
	}

	public bool hasTexture(int wall) {
		return getTexture(wall) != -1;
	}

	public bool hasTexture(Direction dir) {
		return getTexture(dir.Num) != -1;
	}

	public bool hasDefaultTexture(Direction dir) {
		return getTexture(dir) == 0;
	}

	public Block setTexture(int textureId, int wall) {
		textures[wall] = textureId;
		return this;
	}

	public Block setTexture(int textureId, params Direction[] dirs) {
		foreach(var d in dirs) {
			textures[d.Num] = textureId;
		}

		return this;
	}

	public Block setTexture(params Direction[] dirs) {
		return setTexture(0, dirs);
	}

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