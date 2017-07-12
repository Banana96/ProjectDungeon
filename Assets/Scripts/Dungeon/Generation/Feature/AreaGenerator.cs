using UnityEngine;
using Random = System.Random;

/// <summary>Generates <c>Area</c> objects for generated dungeon.</summary>
public class AreaGenerator : FeatureGenerator {
	private const int MinAreaSize = 2;
	private const int MaxAreaSize = 4;
	private const int MinAreaTries = 32;
	private const int MaxAreaTries = 64;

	/// <summary>Generate random area.</summary>
	private Area randomArea(Dungeon dungeon, Random rng) {
		int w = rng.Next(MinAreaSize, MaxAreaSize),
			h = rng.Next(MinAreaSize, MaxAreaSize);

		int x = rng.Next(0, dungeon.Width - w),
			y = rng.Next(0, dungeon.Height - h);

		return new Area(x, y, w, h);
	}

	public bool generate(Dungeon dungeon, Random rng) {
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

		return true;
	}
}