using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public class DungeonGenerator : DungeonBuilder {
	public string seed = "";

	private readonly List<FeatureGenerator> features = new List<FeatureGenerator>();
	private Random rng { get; set; }

	public DungeonGenerator(Dungeon d) : base(d) {}

	protected override void Work() {
		long seedResult;

		if(seed.Length > 0) {
			if(!long.TryParse(seed, out seedResult)) {
                foreach(var c in seed) {
			        seedResult = seedResult + c;
			    }
			}
		} else {
			seedResult = new Random().Next();
			seed = seedResult.ToString();
		}

		rng = new Random((int)seedResult);

		int width = rng.Next(Dungeon.MinSize, Dungeon.MaxSize),
            height = rng.Next(Dungeon.MinSize, Dungeon.MaxSize);

		dungeon.InitBlockArray(width, height);

		features.AddRange(new FeatureGenerator[] {
			new AreaGenerator(),
			new CorridorGenerator(),
			new AreaLinker(),
			new DeadEndsRemover(),
			new PlayerSpawner()
//			new BlockDecorator(),
//			new ChestGenerator()
		});

		foreach(var f in features) {
			Debug.Assert(f.generate(dungeon, rng), f + " returned false");
		}
	}
}