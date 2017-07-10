using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

/// <summary>Joins areas in dungeon with corridors.</summary>
public class AreaLinker : FeatureGenerator {
	
	/// No tuples in Unity, so I have to do this
	private class Edge {
		public readonly Position pos;
		public readonly Direction dir;

		public Edge(Position p, Direction d) {
			pos = p;
			dir = d;
		}
	}

	public override bool generate(Dungeon dungeon, Random rng) {
		var doorTex = dungeon.textures.doorOpen;

		foreach(var area in dungeon.areas) {
			var edges = new List<Edge>();

			// Write all possible edges
			for(var x = area.minX; x < area.maxX; x++) {
				edges.Add(new Edge(new Position(x, area.minY), Direction.South));
				edges.Add(new Edge(new Position(x, area.maxY), Direction.North));
			}

			for(var y = area.minY; y < area.maxY; y++) {
				edges.Add(new Edge(new Position(area.minX, y), Direction.West));
				edges.Add(new Edge(new Position(area.maxX, y), Direction.East));
			}

			// Remove edges with no connection or connection to another area
			for(var i = 0; i < edges.Count; i++) {
				var e = edges[i];
				var adj = dungeon.getAdjBlock(e.pos, e.dir);

				if(adj == null || adj.isAreaBlock()) {
					edges.Remove(e);
					i--;
				}
			}

			// If something went wrong, return false
			if(edges.Count == 0) {
				Debug.Assert(false, "No edges to link area to");
				return false;
			}

			// Select random edge
			var edge = U.RandArrElem(edges, rng);

			// Get actual blocks and validate
			var block1 = dungeon.getBlock(edge.pos);
			var block2 = dungeon.getAdjBlock(edge.pos, edge.dir);

			// Set passable and door texture
			if(block1 != null && block2 != null) {
				block1
					.setPassable(edge.dir)
					.setTexture(doorTex, edge.dir);

				block2
					.setPassable(edge.dir.GetOpposite())
					.setTexture(doorTex, edge.dir.GetOpposite());
			} else {
				Debug.Assert(false, "Selected edge is invalid");
			}
		}

		return true;
	}
}