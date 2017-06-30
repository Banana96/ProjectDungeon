using System;

public class AreaGenerator : FeatureGenerator {
	public static readonly int MinAreaSize = 3;
	public static readonly int MaxAreaSize = 5;
	public static readonly int MinAreaTries = 32;
	public static readonly int MaxAreaTries = 64;

	public Area randomArea(Dungeon dungeon, Random rng) {
		int w = rng.Next(MinAreaSize, MaxAreaSize),
			h = rng.Next(MinAreaSize, MaxAreaSize);

		int x = rng.Next(0, dungeon.Width - w),
			y = rng.Next(0, dungeon.Height - h);

		return new Area(x, y, w, h);
	}

	public override bool generate(Dungeon dungeon, Random rng) {
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

		foreach(var a in dungeon.areas) {
			Area.DrawOnDungeon(dungeon, a);
		}

		return true;
	}
}
