using Random = System.Random;

public partial class DungeonGenerator {
	private const int MinAreaSize = 3;
	private const int MaxAreaSize = 5;
	private const int MinAreaTries = 64;
	private const int MaxAreaTries = 128;

	/// <summary>Generate random area.</summary>
	private Area randomArea(Dungeon dungeon, Random rng) {
		int w = rng.Next(MinAreaSize, MaxAreaSize),
			h = rng.Next(MinAreaSize, MaxAreaSize);

		int x = rng.Next(0, dungeon.Width - w),
			y = rng.Next(0, dungeon.Height - h);

		return new Area(x, y, w, h);
	}

	private void GenerateAreas() {
		dungeon.areas.Add(randomArea(dungeon, rng));

		var tries = rng.Next(MinAreaTries, MaxAreaTries);

		for(var i = 0; i <= tries; i++) {
			var ra = randomArea(dungeon, rng);
			var touch = false;

			foreach(var ia in dungeon.areas) {
				if(ra.touches(ia)) {
					touch = true;
				}
			}

			if(!touch) {
				dungeon.areas.Add(ra);
			}
		}

		foreach(var area in dungeon.areas) {
			area.draw(dungeon);
		}
	}
}
