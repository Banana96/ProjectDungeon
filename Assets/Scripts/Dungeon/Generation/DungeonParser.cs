using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DungeonParser : DungeonBuilder {
	private bool sizeSet;
	private bool fitWalls = true;

	private readonly string rawCode;

	public DungeonParser(Dungeon d, string code) : base(d) {
		rawCode = code;
	}

	private static Position argsPosition(string xArg, string yArg) {
		return new Position(int.Parse(xArg), int.Parse(yArg));
	}

	private Direction[] argsWalls(string dirsArgs) {
		var dirs = new List<Direction>();

		foreach(var d in dirsArgs.ToUpper()) {
			Direction dir;

			switch(d) {
				case 'N': dir = Direction.North; break;
				case 'E': dir = Direction.East; break;
				case 'S': dir = Direction.South; break;
				case 'W': dir = Direction.West; break;
				default: continue;
			}

			if(!dirs.Contains(dir)) {
				dirs.Add(dir);
			}
		}

		return dirs.ToArray();
	}

	private int[] argsWallNums(string dirsArgs) {
		var dirs = new List<int>();

		foreach(var d in dirsArgs.ToUpper()) {
			int dir;

			switch(d) {
				case 'N': dir = Direction.North.Num; break;
				case 'E': dir = Direction.East.Num; break;
				case 'S': dir = Direction.South.Num; break;
				case 'W': dir = Direction.West.Num; break;
				case 'F': dir = 4; break;
				case 'C': dir = 5; break;
				default: continue;
			}

			if(!dirs.Contains(dir)) {
				dirs.Add(dir);
			}
		}

		return dirs.ToArray();
	}

	private void SetDungeonSize(string xArg, string yArg) {
		if(!sizeSet) {
			var size = argsPosition(xArg, yArg);
			dungeon.InitBlockArray(size.x, size.y);
			sizeSet = true;
		}
	}

	private void SetFitWalls(string arg) {
		fitWalls = (arg == "T");
	}

	private void SetBlock(string xArg, string yArg) {
		var p = argsPosition(xArg, yArg);

		if(dungeon.isPosValid(p)) {
			var block = dungeon.setBlock(p);

			if(fitWalls) {
				var adj = dungeon.getAdjBlocks(p);

				foreach(var dir in Direction.All) {
					if(adj[dir.Num] != null) {
						adj[dir.Num]
							.setPassable(dir.GetOpposite())
							.setTexture(dir.GetOpposite());

						block
							.setPassable(dir)
							.removeTexture(dir);
					}
				}
			}
		}
	}

	private void SetArea(string x1Arg, string y1Arg, string x2Arg, string y2Arg) {
		var c1 = argsPosition(x1Arg, y1Arg);
		var c2 = argsPosition(x2Arg, y2Arg);

		Area.DrawOnDungeon(dungeon, new Area(c1.x, c1.y, c2.x, c2.y));
	}

	private void SetBlockTexture(string xArg, string yArg, string wallArg, string texArg = "0") {
		var block = dungeon.getBlock(argsPosition(xArg, yArg));

		if(block != null) {
			var walls = argsWallNums(wallArg);

			var texture = 0;

			if(!int.TryParse(texArg, out texture)) {
				if(texArg == "N") { //None
					texture = -1;
				} else if(texArg == "D") { //Default
					texture = 0;
				}
			}

			foreach(var wall in walls) {
				block.setTexture(texture, wall);
			}
		}		
	}

	private void SetBlockPassable(string xArg, string yArg, string wallArg, string boolArg = "T") {
		var block = dungeon.getBlock(argsPosition(xArg, yArg));

		if(block != null) {
			var walls = argsWalls(wallArg);

			if(boolArg == "T") {
				block.setPassable(walls);
			} else {
				block.setUnpassable(walls);
			}
		}
	}

	private void SetDoor(string xArg, string yArg, string dirArg) {
		var pos = argsPosition(xArg, yArg);
		var dir = argsWalls(dirArg)[0];

		Block block1 = dungeon.getBlock(pos),
			block2 = dungeon.getBlock(pos.copy().add(dir));

		if(block1 != null && block2 != null) {
			block1
				.setTexture(dungeon.textures.doorOpen, dir)
				.setSpecial(dir)
				.setPassable(dir);

			block2
				.setTexture(dungeon.textures.doorOpen, dir.GetOpposite())
				.setSpecial(dir.GetOpposite())
				.setPassable(dir.GetOpposite());
		}
	}

	private string trimmedRawCode {
		get {
			var sb = new StringBuilder(rawCode.Length);

			foreach(var c in rawCode) {
				if(!char.IsWhiteSpace(c)) {
					sb.Append(c);
				}
			}

			return sb.ToString();
		}
	}

	protected override void Work() {
		foreach(var instr in trimmedRawCode.Split(';')) {
			var args = instr.Split(',');

			if(string.IsNullOrEmpty(instr)) {
				continue;
			}

			switch(args[0].ToUpper()) {
				case "S": //Size
					SetDungeonSize(args[1], args[2]);
					break;

				case "F": //Fit Walls
					SetFitWalls(args[1]);
					break;

				case "B": //Block
					SetBlock(args[1], args[2]);
					break;

				case "A": //Area
					SetArea(args[1], args[2], args[3], args[4]);
					break;

				case "T": //BlockTexture
					SetBlockTexture(args[1], args[2], args[3]);
					break;

				case "AT": //AreaTexture

					break;

				case "P": //Passable
					SetBlockPassable(args[1], args[2], args[3]);
					break;

				case "AP": //AreaPassable

					break;

				case "D": //Door
					SetDoor(args[1], args[2], args[3]);
					break;

				default:
					Debug.Log("Unknown instruction \'" + instr + "\'");
					break;
			}
		}
	}
}