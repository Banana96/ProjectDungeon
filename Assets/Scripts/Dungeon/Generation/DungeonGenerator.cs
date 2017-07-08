using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

/// <summary>Builds procedurally generated dungeon from given or random seed.</summary>
public class DungeonGenerator : DungeonBuilder {
	public DungeonGenerator(Dungeon d) : base(d) { }

	protected override void Work() {
		const string seed = "";
		long seedResult;

		if(seed.Length > 0) {
			if(!long.TryParse(seed, out seedResult)) {
				foreach(var c in seed) {
					seedResult = seedResult + c;
				}
			}
		} else {
			seedResult = new Random().Next();
		}

		var rng = new Random((int) seedResult);

		int width = rng.Next(Dungeon.MinSize, Dungeon.MaxSize),
			height = rng.Next(Dungeon.MinSize, Dungeon.MaxSize);

		dungeon.InitBlockArray(width, height);

		var features = new List<FeatureGenerator> {
			new AreaGenerator(),
			new CorridorGenerator(),
			new AreaLinker(),
//			new DeadEndsRemover(),
			new RandomPlayerSpawner()
		};

		foreach(var f in features) {
			var featGen = f.generate(dungeon, rng);
			Debug.Assert(featGen, f + " returned false");
		}

		if(Application.isEditor) {
			var integrity = FloodFill.BlockIntegrityTest(dungeon);
			Debug.Assert(integrity, "Generated dungeon is fractured!");
		}
	}
}