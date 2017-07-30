/// <summary>Represents a single block cell in dungeon.</summary>
public class Block {
	/// <summary>Texture IDs (0 is default, -1 is none).</summary>
	public readonly int[] textures = {0, 0, 0, 0, 0, 0};

	/// <summary>Is side of block special.</summary>
	public readonly bool[] special = {false, false, false, false};

	/// <summary>Is side of block passable.</summary>
	public readonly bool[] passable = {false, false, false, false};

	public bool areaBlock = false;

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

			if(textures[4] > -1 || textures[5] > -1) {
				count++;
			}

			return count;
		}
	}
}
