using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>Spawns chests in dungeon.</summary>
public class ChestGenerator : FeatureGenerator {
	private static readonly int SpawnChance = 50;

	public override bool generate(Dungeon dungeon, Random rng) {
		var areaCount = dungeon.areas.Count;
		var chestCount = Mathf.FloorToInt(areaCount * (SpawnChance / 100));
		chestCount = (chestCount == 0 ? 1 : chestCount);

		var pickedAreas = new List<Area>(chestCount);

		for(var i = 0; i < chestCount;) {
			var randomArea = U.RandArrElem(dungeon.areas, rng);

			if(!pickedAreas.Contains(randomArea)) {
				pickedAreas.Add(randomArea);
				i++;
			}
		}

		Debug.Log("Picked " + pickedAreas.Count + " areas for chests");

		foreach(var a in pickedAreas) {
			Position pos;

			do {
				pos = new Position(rng.Next(a.minX, a.maxX), rng.Next(a.minY, a.maxY));
			} while(dungeon.isNearSpecialBlock(pos));

			dungeon.spawnChest(pos, Direction.North, false);
		}

		return true;
	}
}