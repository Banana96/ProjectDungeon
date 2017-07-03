using UnityEngine;

public class Attack : EntityRequest {
	private Direction direction {
		get { return target as Direction; }
	}

	public Attack(Entity e, Direction dir = null) : base(e, dir) {
		if(dir == null) {
			target = e.facing;
		}
	}

	public override bool validateRequest(Dungeon d) {
		var targetPos = caller.position + direction;
		var callerBlock = d.getBlock(caller.position);
		if(callerBlock != null) {
			if(callerBlock.isPassable(direction)) {
				var block = d.getBlock(targetPos);

				if(block != null) {
					var entity = d.getEntity(targetPos);

					if(entity != null) {
						return true;
					}
				}
			}
		}

		return false;
	}

	public override void onValid(Dungeon d) {
		var targetPos = caller.position + direction;
		var targetEntity = d.getEntity(targetPos);

		if(targetEntity != null) {
			var dmg = targetEntity.receiveHit(caller);

			Debug.Log(targetEntity.publicName +
					(targetEntity.invincible
						? " took " + dmg + " damage"
						: "'s health: " + targetEntity.currentHealth + "/" + targetEntity.maxHealth)
			);
		}
	}

	public override void onInvalid(Dungeon d) {
		Debug.Log("You swing your weapon in the air. What did you expect?");
	}
}