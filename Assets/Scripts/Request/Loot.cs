public class Loot : EntityRequest {
	private Direction direction {
		get { return target as Direction; }
	}

	public Loot(Entity e, Direction dir = null) : base(e, dir) {
		if(target == null) {
			target = e.facing;
		}
	}

	public override bool validateRequest(Dungeon d) {
		var entity = d.getEntity(caller.position + direction);

		if(entity != null) {
			var chest = entity as Chest;

			if(chest != null) {
				//TODO
			}
		}

		return false;
	}

	public override void onValid(Dungeon d) { }

	public override void onInvalid(Dungeon d) { }
}