using System;
using UnityEngine;
using System.Collections.Generic;

public class Dungeon : MonoBehaviour {
	public const int MinSize = 8;
	public const int MaxSize = 24;

	private const string EntitiesObjName = "_entities";

	[SerializeField] private Block[,] blocks;
	[SerializeField] private GameObject entitiesRoot;

	public int Width {
		get { return blocks != null ? blocks.GetLength(0) : 0; }
	}

	public int Height {
		get { return blocks != null ? blocks.GetLength(1) : 0; }
	}

	[SerializeField] public readonly List<Area> areas = new List<Area>();
	[SerializeField] private readonly List<Entity> entities = new List<Entity>();
	[SerializeField] public readonly TextureSet textures = TextureSet.Default;
	[SerializeField] private readonly Position playerSpawn = new Position();

	void Awake() {
		var entTsf = transform.Find(EntitiesObjName);
		entitiesRoot = entTsf != null
			? entTsf.gameObject
			: new GameObject(EntitiesObjName);

		entitiesRoot.transform.parent = transform;
	}

	public void InitBlockArray(int width, int height, bool resizing = false) {
		int newWidth = Mathf.Clamp(width, MinSize, MaxSize),
			newHeight = Mathf.Clamp(height, MinSize, MaxSize);

		var newArray = new Block[newWidth, newHeight];

		if(resizing && blocks != null) {
			for(var x = 0; x < Mathf.Min(Width, newWidth); x++) {
				for(var y = 0; y < Mathf.Min(Height, newHeight); y++) {
					newArray[x, y] = blocks[x, y];
				}
			}
		}

		blocks = newArray;
	}

	public void ForEachPosition(Action<Position> action) {
		for(var x = 0; x < Width; x++) {
			for(var y = 0; y < Height; y++) {
				action.Invoke(new Position(x,y));
			}
		}
	}

	public void ForEachBlock(Action<Position, Block> action) {
		ForEachPosition(delegate(Position pos) {
			action.Invoke(pos, getBlock(pos));
		});
	}

	public void ForEachExistingBlock(Action<Position, Block> action) {
		ForEachPosition(delegate(Position pos) {
			var block = getBlock(pos);
			if(block != null) {
				action.Invoke(pos, block);
			}
		});
	}

	private bool isPosValid(int x, int z) {
		return x >= 0 && x < Width && z >= 0 && z < Height;
	}

	public bool isPosValid(Position p) {
		return p.x >= 0 && p.x < Width && p.y >= 0 && p.y < Height;
	}

	private bool isMoveValid(Position p, Direction d) {
		Block b1 = getBlock(p), b2 = getAdjBlock(p, d);

		if(b1 != null && b2 != null) {
			if(b1.isPassable(d)) {
				if(b2.isPassable(d.GetOpposite())) {
					if(getEntity(p.add(d)) == null) {
						return true;
					}
				}
			}
		}

		return false;
	}

	public Block getBlock(int x, int z) {
		return blocks[x, z];
	}

	public Block getBlock(Position p) {
		return getBlock(p.x, p.y);
	}

	public Block getAdjBlock(Position p, Direction d) {
		var ap = new Position(p.x, p.y).add(d);
		return isPosValid(ap) ? getBlock(ap) : null;
	}

	public Block[] getAdjBlocks(Position p) {
		var adj = new Block[4];

		foreach(var d in Direction.All) {
			adj[d.Num] = getAdjBlock(p, d);
		}

		return adj;
	}

	public List<Direction> getNullAdjBlocks(Position p) {
		var adj = getAdjBlocks(p);
		var free = new List<Direction>();

		foreach(var dir in Direction.All) {
			if(adj[dir.Num] == null && isPosValid(p.copy().add(dir))) {
				free.Add(dir);
			}
		}

		return free;
	}

	public bool isNearSpecialBlock(Position p) {
		var adj = getAdjBlocks(p);

		foreach(var d in Direction.All) {
			if(adj[d.Num] != null) {
				if(adj[d.Num].isSpecial(d)) {
					return true;
				}
			}
		}

		return false;
	}
	private Position firstBlockFreePos() {
		var p = new Position();

		for(p.x = 0; p.x < Width; p.x++) {
			for(p.y = 0; p.y < Height; p.y++) {
				var b = getBlock(p);
				if(b != null) {
					return p;
				}
			}
		}

		return null;
	}

	public Block setBlock(Position p) {
		if(isPosValid(p)) {
			return blocks[p.x, p.y] ?? (blocks[p.x, p.y] = new Block());
		}

		return null;
	}

	public void removeBlock(Position p) {
		if(isPosValid(p)) {
			blocks[p.x, p.y] = null;
		}
	}

	public List<T> getEntities<T>() where T : Entity {
		var ents = new List<T>();

		foreach(var e in entities) {
			var ent = e as T;

			if(ent != null) {
				ents.Add(ent);
			}
		}

		return ents;
	}

	private T getEntity<T>() where T : Entity {
		foreach(var e in entities) {
			var ent = e as T;

			if(ent != null) {
				return ent;
			}
		}

		return null;
	}

	public Entity getEntity(Position pos) {
		foreach(var ent in entities) {
			if(Position.Compare(ent.position, pos)) {
				return ent;
			}
		}

		return null;
	}

	private T spawn<T>(Position p, Direction facing) where T : Entity {
		var go = new GameObject();
		var e = go.AddComponent<T>();

		var posOffset = Vector3.zero;

		e.transform.parent = entitiesRoot.transform;
		e.dungeon = this;
		e.position = p;
		e.transform.position += posOffset;
		e.name = typeof(T).Name;
		e.facing = facing;

		entities.Add(e);

		return e;
	}

	private T spawn<T>(Position p) where T : Entity {
		return spawn<T>(p, Direction.North);
	}

	public Player getPlayer() {
		return getEntity<Player>();
	}

	public void setPlayerSpawn(Position p) {
		playerSpawn.x = p.x;
		playerSpawn.y = p.y;
	}

	public Player spawnPlayer(Direction facing) {
		return getPlayer() ?? spawn<Player>(playerSpawn, facing);
	}

	public void autoInitPlayer() {
		if(getPlayer() == null) {
			var pos = firstBlockFreePos();
			var dir = Direction.North;

			foreach(var d in Direction.All) {
				if(getBlock(pos).isPassable(d)) {
					dir = d;
					break;
				}
			}

			spawnPlayer(dir);
		}
	}

	public Chest spawnChest(Position p, Direction dir, bool locked, params Item[] content) {
		var chest = spawn<Chest>(p, dir);

		if(chest != null) {
			chest.isLocked = locked;
			chest.content.itemIn(content);

			chest.transform.Rotate(90, 0, 0);
			chest.transform.position = new Vector3(p.x, 0, p.y);

			return chest;
		}

		return null;
	}

	public Monster spawnMonster(Position p, MonsterTemplate template) {
		var m = spawn<Monster>(p);

		try {
			if(m != null) {
				m.Build(template);
			}
		} catch {
			Debug.LogError("Failed to instantiate monster");
		}

		return m;
	}

	public Monster spawnMonster(Position p, string id) {
		return spawnMonster(p, Monster.GetTemplate(id));
	}

	public void requestUpdate(PlayerInterface pi) {
		foreach(var ent in entities) {
			ent.pushRequest();

			if(ent.hasRequest) {
				var req = ent.request;
				var valid = req.validateRequest(this);

				if(valid) {
					req.onValid(this);
				} else {
					req.onInvalid(this);
				}

				if(req.isCallerPlayer) {
					pi.requestResponse = valid ? Response.Allow : Response.Deny;
				}

				ent.clearRequest();
			}
		}
	}
}