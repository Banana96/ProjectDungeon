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

		var features = new List<FeatureGenerator>(new FeatureGenerator[] {
			new AreaGenerator(),
			new CorridorGenerator(),
			new AreaLinker(),
			new DeadEndsRemover(),
			new RandomPlayerSpawner()
//			new BlockDecorator(),
//			new ChestGenerator()
		});

		foreach(var f in features) {
			Debug.Assert(f.generate(dungeon, rng), f + " returned false");
		}
	}
}