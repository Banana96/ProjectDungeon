using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// <c>Dungeon</c> class represents a complete level: its blocks, areas, entities and spawn positions,
/// and allows to modify them according to the game rules.
/// </summary>
[ExecuteInEditMode]
public class Dungeon : MonoBehaviour {
	#region Singleton instance
	private static Dungeon _inst;

	public static Dungeon Instance {
		get {
			return _inst;
		}

		private set {
			if(_inst == null) {
				_inst = value;
			}
		}
	}
	#endregion

	#region Constants
	/// <summary>Minimal size of the level.</summary>
	public const int MinSize = 16;

	/// <summary>Maximal size of the level.</summary>
	public const int MaxSize = 32;

	/// <summary>Name of object containing this <c>Dungeon</c> entities.</summary>
	private const string EntitiesObjName = "_entities";
	#endregion

	#region Fields and properties
	/// <summary>Array of <c>Block</c>s in cartographical order.</summary>
	[SerializeField] private Block[,] blocks;

	/// <summary>Reference to </summary>
	[SerializeField] private GameObject entitiesRoot;

	/// <summary>Array of blocks width.</summary>
	public int Width => blocks?.GetLength(0) ?? 0;

	/// <summary>Array of blocks height.</summary>
	public int Height => blocks?.GetLength(1) ?? 0;

	/// <summary>List of areas. It is populated by an instance of <c>DungeonBuilder</c>.</summary>
	[SerializeField] public readonly List<Area> areas = new List<Area>();

	/// <summary>List of all <c>Entity</c> instances on this level.</summary>
	[SerializeField] private readonly List<Entity> entities = new List<Entity>();

	/// <summary><c>TextureSet</c> used by this <c>Dungeon</c>. It's used by <c>DungeonBuilder</c>s and <c>DungeonMeshBuilder</c>.</summary>
	[SerializeField] public readonly TextureSet textures = TextureSet.Default;
	#endregion

	#region Initialization
	void Awake() {
		if(_inst == null) {
			_inst = this;
		} else {
			Debug.LogWarning("An instance of Dungeon already exists");
			enabled = false;
			return;
		}

		gameObject.transform.position = Vector3.zero;
		gameObject.isStatic = true;

		var entTsf = transform.Find(EntitiesObjName);
		entitiesRoot = entTsf?.gameObject ?? new GameObject(EntitiesObjName);

		entitiesRoot.transform.parent = transform;
	}

	/// <summary>Initializes empty, or resizes existing array of blocks.
	/// <c>width</c> and <c>height</c> are clamped according to min/max size limits.</summary>
	/// <param name="width">Level width</param>
	/// <param name="height">Level height</param>
	/// <param name="resizing">Don't erase existing blocks if <c>true</c></param>
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
	#endregion

	#region Position iterators
	/// <summary>Execute <c>action</c> for each possible position on level.</summary>
	/// <param name="action">Action performed on each <c>Position</c></param>
	public void ForEachPosition(Action<Position> action) {
		for(var x = 0; x < Width; x++) {
			for(var y = 0; y < Height; y++) {
				action.Invoke(new Position(x, y));
			}
		}
	}

	/// <summary>Execute <c>action</c> for each possible block on level.</summary>
	/// /// <param name="action">Action performed on each <c>Block</c> in <c>Position</c></param>
	public void ForEachBlock(Action<Position, Block> action) {
		ForEachPosition(delegate (Position pos) { action.Invoke(pos, getBlock(pos)); });
	}

	/// <summary>Execute <c>action</c> for each existing block on level.</summary>
	/// /// <param name="action">Action performed on each existing <c>Block</c></param>
	public void ForEachExistingBlock(Action<Position, Block> action) {
		ForEachPosition(delegate (Position pos) {
			var block = getBlock(pos);
			if(block != null) {
				action.Invoke(pos, block);
			}
		});
	}
	#endregion

	#region Block operators

	/// <summary>Validates if given <c>Position</c> is within level bounds.</summary>
	/// <returns>Is position in level bounds.</returns>
	public bool isPosValid(Position p) {
		return p.x >= 0 && p.x < Width && p.y >= 0 && p.y < Height;
	}

	/// <summary>Validates if movement is possible from given <c>Position</c> in given <c>Direction</c>.</summary>
	/// <returns>Is movement valid, considering wall and <c>Entity</c> collisions.</returns>
	private bool isMoveValid(Position p, Direction d) {
		Block b1 = getBlock(p), b2 = getAdjBlock(p, d);

		if(b1 != null && b2 != null) {
			if(b1.isPassable(d)) {
				if(b2.isPassable(d.Opposite)) {
					if(getEntity(p.add(d)) == null) {
						return true;
					}
				}
			}
		}

		return false;
	}

	/// <summary>Returns block from given coordinates WITHOUT checking bounds.</summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <returns>Block on given coordinates, or <c>null</c>.</returns>
	public Block getBlock(int x, int y) {
		return blocks[x, y];
	}

	/// <summary>Retrieves block from given <c>Position</c> WITHOUT checking bounds.</summary>
	/// <param name="p">Block position</param>
	/// <returns>Block on given <c>Position</c> or <c>null</c>.</returns>
	public Block getBlock(Position p) {
		return getBlock(p.x, p.y);
	}

	/// <summary>Retrieves block adjacent in given direction from given position.</summary>
	/// <param name="p">Origin position.</param>
	/// <param name="d">Direction from origin.</param>
	/// <returns>Adjacent block.</returns>
	public Block getAdjBlock(Position p, Direction d) {
		var ap = p.copy().add(d);
		return isPosValid(ap) ? getBlock(ap) : null;
	}

	/// <summary>Retrieves array of adjacent blocks from given position.</summary>
	/// <param name="p">Origin position.</param>
	/// <returns>Array containing <c>Block</c>s in clockwise order.</returns>
	public Block[] getAdjBlocks(Position p) {
		var adj = new Block[4];

		foreach(var d in Direction.All) {
			adj[d.Num] = getAdjBlock(p, d);
		}

		return adj;
	}

	/// <summary>Which adjacent position has no <c>Block</c>.</summary>
	/// <param name="p">Origin position.</param>
	/// <returns>List of directions in clockwise order.</returns>
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

	/// <summary>Is given position adjacent to a special block.</summary>
	/// <param name="p">Origin position.</param>
	/// <returns>Is origin position adjacent to a special block.</returns>
	/// <see cref="Block"/>
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

	/// <summary>Position of first occurence of existing <c>Block</c> in rows/columns order.</summary>
	/// <returns>Position of first block.</returns>
	public Position firstBlockFreePos() {
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

	/// <summary>Sets new <c>Block</c>, if it doesn't already exist in given place
	/// WITH level bounds check.</summary>
	/// <param name="p">Position of set block.</param>
	/// <returns>Created or already existing block, when in bounds; otherwise <c>null</c></returns>
	public Block setBlock(Position p) {
		if(isPosValid(p)) {
			return blocks[p.x, p.y] ?? (blocks[p.x, p.y] = new Block());
		}

		return null;
	}

	/// <summary>Removes <c>Block</c> from given position. WITH level bounds check.</summary>
	/// <param name="p">Position to remove block from.</param>
	public void removeBlock(Position p) {
		if(isPosValid(p)) {
			blocks[p.x, p.y] = null;
		}
	}
	#endregion

	#region Entity finders
	/// <summary>Search for all entities of given type.</summary>
	/// <typeparam name="T">Type of entity.</typeparam>
	/// <returns>List of entities of given type.</returns>
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

	/// <summary>Search for one entity of given type.</summary>
	/// <typeparam name="T">Type of entity.</typeparam>
	/// <returns>First found entity of given type or null otherwise.</returns>
	private T getEntity<T>() where T : Entity {
		foreach(var e in entities) {
			var ent = e as T;

			if(ent != null) {
				return ent;
			}
		}

		return null;
	}

	/// <summary>Search for entity in given position</summary>
	/// <param name="pos">Position of entity</param>
	/// <returns>Entity reference or <c>null</c></returns>
	public Entity getEntity(Position pos) {
		foreach(var ent in entities) {
			if(Position.Compare(ent.position, pos)) {
				return ent;
			}
		}

		return null;
	}

	/// <summary>Get <c>Player</c> instance</summary>
	/// <returns>Player instance or null</returns>
	public Player getPlayer()
	{
		return getEntity<Player>();
	}
	#endregion

	#region Entity spawners
	/// <summary>Spawn new instance of <c>Entity</c>.</summary>
	/// <typeparam name="T">Type of entity.</typeparam>
	/// <param name="p">New entity position.</param>
	/// <param name="facing">New entity direction.</param>
	/// <returns>Created entity reference.</returns>
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

	/// <summary>Spawn new instance of <c>Entity</c>, that faces north.</summary>
	/// <typeparam name="T">Type of entity.</typeparam>
	/// <param name="p">New entity position.</param>
	/// <returns>Created entity reference.</returns>
	private T spawn<T>(Position p) where T : Entity {
		return spawn<T>(p, Direction.North);
	}

	/// <summary>Spawn new <c>Player</c> instance if one doesn't exist already.</summary>
	/// <param name="p">New player position.</param>
	/// <param name="facing">New player direction.</param>
	/// <returns>Created player reference.</returns>
	public Player spawnPlayer(Position p, Direction facing) {
		return getPlayer() ?? spawn<Player>(p, facing);
	}

	/// <summary>Create new player instance if one doesn't exist already.
	/// Place new <c>Player</c> on first encountered <c>Block</c> and facing north.</summary>
	/// <remarks>Should be used only by <c>DungeonBuilder</c></remarks>
	/// <see cref="DungeonBuilder"/>
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

			spawnPlayer(pos, dir);
		}
	}

	/// <summary>Create new chest instance.</summary>
	/// <param name="p">Position</param>
	/// <param name="dir">Direction</param>
	/// <param name="locked">Is chest locked</param>
	/// <param name="content">Chest contents</param>
	/// <returns>Created chest reference.</returns>
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

	/// <summary>Create new monster instance of given <c>MonsterTemplate</c>.</summary>
	/// <param name="p">Position</param>
	/// <param name="template">Template</param>
	/// <returns>Created monster referenceo or null if template is invalid or <c>null</c>.</returns>
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

	/// <summary>Create new monster instance of given template name.</summary>
	/// <param name="p">Position</param>
	/// <param name="id">Template id (name)</param>
	/// <returns>Created monster referenceo or null if template is invalid or <c>null</c>.</returns>
	public Monster spawnMonster(Position p, string id) {
		return spawnMonster(p, Monster.GetTemplate(id));
	}

	#endregion

	#region Logic update
	/// <summary>
	/// Update whole level logic queue and force every other <c>Entity</c> to update.
	/// Used in <c>PlayerInterface</c>, when player requests an update.
	/// </summary>
	/// <param name="pi">Player interface (used to </param>
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
	#endregion
}
