using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>Builds procedurally generated dungeon from given or random seed.</summary>
public partial class DungeonGenerator : DungeonBuilder {
	public int seed { get; private set; }
	private Random rng;

	public DungeonGenerator(Dungeon d, int s = 0) : base(d) {
		if(s == 0) {
			s = new Random().Next();
		}

		seed = s;
		rng = new Random(seed);

		buildProcess += InitDimension;
		buildProcess += GenerateAreas;
		buildProcess += GenerateCorridors;
		buildProcess += LinkAreas;
		buildProcess += RemoveDeadEnds;
		buildProcess += SpawnPlayer;
		buildProcess += DecorateBlocks;
		buildProcess += BuildMesh;

		if(Application.isEditor || Debug.isDebugBuild) {
			buildProcess += IntegrityCheck;
		}
	}

	private void InitDimension() {
		int width = rng.Next(Dungeon.MinSize, Dungeon.MaxSize),
			height = rng.Next(Dungeon.MinSize, Dungeon.MaxSize);

		dungeon.InitBlockArray(width, height);

		Debug.Log($"Dungeon size: {width}x{height}");
	}

	private void IntegrityCheck() {
		var result = FloodFill.BlockIntegrityTest(dungeon);
		Debug.Assert(result, "Generated dungeon is fractured!");
	}
}
