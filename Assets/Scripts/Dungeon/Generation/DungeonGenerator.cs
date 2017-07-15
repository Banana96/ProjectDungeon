using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>Builds procedurally generated dungeon from given or random seed.</summary>
public class DungeonGenerator : DungeonBuilder {
	private readonly string seed;
	private Random rng;
	private List<FeatureGenerator> features;

	public DungeonGenerator(Dungeon d, string s = "") : base(d) {
		seed = s;

		buildProcess += Init;	
		buildProcess += Generate;
		buildProcess += BuildMesh;
	}

	/// <summary>Converts entered seed string to int seed, or returns random one.</summary>
	private int getSeedResult() {
		if(seed.Length == 0) {
			return new Random().Next();
		}

		var result = 0;
		var i = 0;

		foreach(var letter in seed) {
			result += letter * (int)Mathf.Pow(10, i++);
		}

		return result;
	}

	private void Init() {
		rng = new Random(getSeedResult());

		features = new List<FeatureGenerator> {
			new AreaGenerator(),
			new CorridorGenerator(),
			new AreaLinker(),
			new DeadEndsRemover(),
			new RandomPlayerSpawner()
		};

		int width = rng.Next(Dungeon.MinSize, Dungeon.MaxSize),
			height = rng.Next(Dungeon.MinSize, Dungeon.MaxSize);

		dungeon.InitBlockArray(width, height);

		Debug.Log($"Dungeon size: {width}x{height}");
	}

	private void Generate() {
		foreach(var f in features) {
			Debug.Assert(f.generate(dungeon, rng), f + " failed");
		}

		if(Application.isEditor) {
			var integrity = FloodFill.BlockIntegrityTest(dungeon);
			Debug.Assert(integrity, "Generated dungeon is fractured!");
			Application.Quit();
		}
	}
}
