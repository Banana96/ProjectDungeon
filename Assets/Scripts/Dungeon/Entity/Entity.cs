using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
	private Dungeon _dng;

	public Dungeon dungeon {
		private get { return _dng; }
		set { if(_dng == null) { _dng = value; } }
	}

	public EntityRequest request { get; protected set; }

	public EntityTemplate template;
	
	public string publicName { get { return template.publicName; } }

	public bool invincible { get { return template.invincible; } }

	public int maxHealth { get { return template.health; } }
	public int currentHealth { get; protected set; }

	public int minAttack { get { return template.minAttack; } }
	public int maxAttack { get { return template.maxAttack; } }
	public int nextAttack { get { return minAttack + Random.Range(minAttack - 1, maxAttack); } }

	public Direction facing {
		get {
			return Direction.FromAngle(transform.rotation.eulerAngles.y);
		}

		set {
			if(value != null) {
				transform.rotation = Quaternion.Euler(0, value.GetAngle(), 0);
			}
		}
	}

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

	public int receiveHit(Entity ent) {
		var dmg = ent.nextAttack;

		if(!invincible) {
			currentHealth -= dmg;
		}

		return dmg;
	}

	public bool hasRequest { get { return request != null; } }
	public void clearRequest() { request = null; }

	public virtual void pushRequest() {}
}