using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class DungeonMeshBuilder {
	private readonly Vector3[][] QuadVertices = {
		new[] {
			// North
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, 1f, 0.5f),
			new Vector3(0.5f, 1f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f)
		},
		new[] {
			// East
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, 1f, 0.5f),
			new Vector3(0.5f, 1f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f)
		},
		new[] {
			// South
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 1f, -0.5f),
			new Vector3(-0.5f, 1f, -0.5f),
			new Vector3(-0.5f, -0.5f, -0.5f)
		},
		new[] {
			// West
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, 1f, -0.5f),
			new Vector3(-0.5f, 1f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f)
		},
		new[] {
			// Floor
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, -0.5f)
		},
		new[] {
			// Ceiling
			new Vector3(-0.5f, 1f, -0.5f),
			new Vector3(0.5f, 1f, -0.5f),
			new Vector3(0.5f, 1f, 0.5f),
			new Vector3(-0.5f, 1f, 0.5f)
		}
	};

	private readonly Dungeon dungeon;
	private readonly Transform staticMesh;
	private readonly bool buildColliders;

	public DungeonMeshBuilder(Dungeon dng, bool buildColliders = false) {
		if(dng == null) {
			Debug.LogError("No dungeon set to be built!");
		} else {
			dungeon = dng;
			staticMesh = dungeon.gameObject.transform.Find("_staticMesh");
			this.buildColliders = buildColliders;

			if(staticMesh == null) {
				staticMesh = new GameObject("_staticMesh").transform;
				staticMesh.parent = dungeon.transform;
			}
		}
	}

	private Mesh CreateWallMesh(int wall, int x, int z) {
		var m = new Mesh();

		var w = QuadVertices[wall];

		m.vertices = new[] {
			new Vector3(w[0].x + x, w[0].y, w[0].z + z),
			new Vector3(w[1].x + x, w[1].y, w[1].z + z),
			new Vector3(w[2].x + x, w[2].y, w[2].z + z),
			new Vector3(w[3].x + x, w[3].y, w[3].z + z)
		};

		m.uv = new[] {
			new Vector2(0, 0),
			new Vector2(0, 1),
			new Vector2(1, 1),
			new Vector2(1, 0)
		};

		m.triangles = new[] {0, 1, 2, 0, 2, 3};

		return m;
	}

	private Dictionary<int, List<Mesh>> meshGroup;
	private List<Mesh> wallColliders;

	private void PopulateMeshArrays() {
		dungeon.ForEachPosition(delegate(Position pos) {
			var block = dungeon.getBlock(pos);
			if(block != null) {
				var textures = block.getTextures();

				for(var wall = 0; wall < textures.Length; wall++) {
					var texture = textures[wall];
					var genColl = false;

					if(texture >= 0) {
						if(texture == 0) {
							if(wall >= 0 & wall < 4) {
								texture = dungeon.textures.defaultWall;
								genColl = !block.isPassable(wall);
							} else if(wall == 4) {
								texture = dungeon.textures.defaultFloor;
							} else if(wall == 5) {
								texture = dungeon.textures.defaultCeiling;
							}
						}

						if(!meshGroup.ContainsKey(texture)) {
							meshGroup.Add(texture, new List<Mesh>());
						}

						var wallMesh = CreateWallMesh(wall, pos.x, pos.y);

						meshGroup[texture].Add(wallMesh);

						if(genColl) {
							wallColliders.Add(wallMesh);
						}
					}
				}
			}
		});
	}

	private void BuildMeshGroups() {
		foreach(var texture in meshGroup.Keys) {
			var group = meshGroup[texture];

			var meshGroupObject = new GameObject("_meshGroup_" + texture);
			meshGroupObject.transform.parent = staticMesh.transform;

			var mf = meshGroupObject.AddComponent<MeshFilter>();
			var combines = new CombineInstance[group.Count];

			for(var i = 0; i < combines.Length; i++) {
				combines[i].mesh = group[i];
			}

			var shared = new Mesh();
			shared.CombineMeshes(combines, true, false);
			shared.RecalculateNormals();

			mf.sharedMesh = shared;

			var mr = meshGroupObject.AddComponent<MeshRenderer>();
			mr.material = Resources.Load<Material>("Materials/WallEnv");
			mr.material.mainTexture = Resources.Load<Texture>("Textures/Environment/" + texture);

			mr.shadowCastingMode = ShadowCastingMode.Off;
			mr.receiveShadows = false;
		}
	}

	private void BuildColliderGroups() {
		var collCombines = new CombineInstance[wallColliders.Count];

		for(var i = 0; i < collCombines.Length; i++) {
			collCombines[i].mesh = wallColliders[i];
		}

		var sharedColl = new Mesh();
		sharedColl.CombineMeshes(collCombines, true, false);

		dungeon.gameObject.AddComponent<MeshCollider>().sharedMesh = sharedColl;
	}

	public void BuildMesh() {
		meshGroup = new Dictionary<int, List<Mesh>>();
		wallColliders = new List<Mesh>();

		PopulateMeshArrays();
		BuildMeshGroups();

		if(buildColliders) {
			BuildColliderGroups();
		}
	}
}