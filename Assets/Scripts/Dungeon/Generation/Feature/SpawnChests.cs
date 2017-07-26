using System.Collections.Generic;
using UnityEngine;

public partial class DungeonGenerator {
	private const int SpawnChance = 20;

	private void SpawnChests() {
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

		foreach(var a in pickedAreas) {
			Position pos;

			do {
				pos = new Position(rng.Next(a.minX, a.maxX), rng.Next(a.minY, a.maxY));
			} while(dungeon.isNearSpecialBlock(pos));

			dungeon.spawnChest(pos, Direction.North, false);
		}
	}
}
