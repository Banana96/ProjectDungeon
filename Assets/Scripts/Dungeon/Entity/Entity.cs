using System.Collections.Generic;
using UnityEngine;

/// <summary>Entity represents every object, that changes its state or can alter further player decisions.</summary>
public abstract class Entity : MonoBehaviour {
	/// <summary>Dungeon where this entity exists.</summary>
	private Dungeon _dng;

	/// <summary>Safelock for avoiding sudden <c>Dungeon</c> instance change.</summary>
	public Dungeon dungeon {
		private get { return _dng; }
		set {
			if(_dng == null) {
				_dng = value;
			}
		}
	}

	/// <summary>Currently held request. Clears after execution.</summary>
	public EntityRequest request { get; protected set; }

	/// <summary>Template used to create this entity instance.</summary>
	protected EntityTemplate template;

	/// <summary>Name visible for player in-game.</summary>
	public string publicName {
		get { return template.publicName; }
	}

	/// <summary>Can this entity be hurt or killed.</summary>
	public bool invincible {
		get { return template.invincible; }
	}

	/// <summary>Maximum amount of health points</summary>
	public int maxHealth {
		get { return template.health; }
	}

	/// <summary>Current amount of health points</summary>
	public int currentHealth { get; protected set; }

	/// <summary>Minimal attack, that this entity can deal.</summary>
	public int minAttack {
		get { return template.minAttack; }
	}

	/// <summary>Maximal attack, that this entity can deal.</summary>
	public int maxAttack {
		get { return template.maxAttack; }
	}

	/// <summary>Random value in range between <c>minAttack</c> and <c>maxAttack</c> values.</summary>
	public int nextAttack {
		get { return Random.Range(minAttack, maxAttack + 1); }
	}

	/// <summary>Direction, that this entity is facing.</summary>
	public Direction facing {
		get { return Direction.FromAngle(transform.rotation.eulerAngles.y); }

		set {
			if(value != null) {
				transform.rotation = Quaternion.Euler(0, value.GetAngle(), 0);
			}
		}
	}

	/// <summary>In-level position.</summary>
	public Position position {
		get {
			var v = gameObject.transform.position;
			return new Position(Mathf.FloorToInt(v.x), Mathf.FloorToInt(v.z));
		}

		set {
			if(value != null) {
				gameObject.transform.position = new Vector3(value.x, 0, value.y);
			}
		}
	}

	/// <summary>All actions, that this <c>Entity</c> can perform in current state and environment.</summary>
	/// <param name="dir">In which direction action should be performed.</param>
	/// <returns>List of actions.</returns>
	public EntityRequest[] getAvailableActions(Direction dir) {
		var requestList = new List<EntityRequest>() {
			new Attack(this, dir),
			new Loot(this, dir),
			new Move(this, dir)
		};

		for(var i = 0; i < requestList.Count; i++) {
			var req = requestList[i];

			if(!req.validateRequest(dungeon)) {
				requestList.Remove(req);
				i--;
			}
		}

		return requestList.ToArray();
	}

	/// <summary>Method called, when <c>Entity</c> recieved damage from other entities.</summary>
	/// <returns></returns>
	public int receiveHit(Entity ent) {
		var dmg = ent.nextAttack;

		if(!invincible) {
			currentHealth -= dmg;
		}

		return dmg;
	}

	/// <summary>Is this entity requesting an update.</summary>
	/// <see cref="Dungeon.requestUpdate(PlayerInterface)"/>
	public bool hasRequest {
		get { return request != null; }
	}

	/// <summary>Remove this entity's request, if any exists.</summary>
	public void clearRequest() {
		request = null;
	}

	/// <summary>Force this entity to request any possible action.</summary>
	public virtual void pushRequest() { }
}