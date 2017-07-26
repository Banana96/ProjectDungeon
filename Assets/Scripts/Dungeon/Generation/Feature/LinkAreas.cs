using System.Collections.Generic;
using UnityEngine;

using Edge = System.Tuple<Position, Direction>;

public partial class DungeonGenerator {
	private void LinkAreas() {
		var doorTex = dungeon.textures.doorOpen;

		foreach(var area in dungeon.areas) {
			var allEdges = new List<Edge>();

			// Write all possible edges
			for(var x = area.minX; x < area.maxX; x++) {
				allEdges.Add(new Edge(new Position(x, area.minY), Direction.South));
				allEdges.Add(new Edge(new Position(x, area.maxY), Direction.North));
			}

			for(var y = area.minY; y < area.maxY; y++) {
				allEdges.Add(new Edge(new Position(area.minX, y), Direction.West));
				allEdges.Add(new Edge(new Position(area.maxX, y), Direction.East));
			}

			var corridorEdges = new List<Edge>();

			// Remove edges with no connection or determine connections to another area otherwise
			for(var i = 0; i < allEdges.Count; i++) {
				var edge = allEdges[i];
				var adj = dungeon.getAdjBlock(edge.Item1, edge.Item2);

				if(adj == null) {
					allEdges.Remove(edge);
					i--;
				} else if(adj.areaBlock) {
					corridorEdges.Add(edge);
				}
			}

			var usedEdgeList = corridorEdges;

			// If there are no corridor blocks nearby, select to any block
			if(corridorEdges.Count == 0) {
				usedEdgeList = allEdges;
			}

			try {
				// Select random edge
				var randomEdge = U.RandArrElem(usedEdgeList, rng);

				// Get actual blocks and validate
				var block1 = dungeon.getBlock(randomEdge.Item1);
				var block2 = dungeon.getAdjBlock(randomEdge.Item1, randomEdge.Item2);

				// Set passable and door texture
				if(block1 != null && block2 != null) {
					block1
						.setPassable(randomEdge.Item2)
						.setTexture(doorTex, randomEdge.Item2);

					block2
						.setPassable(randomEdge.Item2.Opposite)
						.setTexture(doorTex, randomEdge.Item2.Opposite);
				} else {
					Debug.Assert(false, "Selected edge is invalid");
				}
			} catch {
				Debug.LogError(this + " failed");
				return;
			}
		}
	}
}
