public class Move : EntityRequest {
	private Direction dir => target as Direction;

	public Move(Entity e, Direction dir = null) : base(e, dir) {
		if(dir == null) {
			target = e.facing;
		}
	}

	public override bool validateRequest(Dungeon d) {
		var currentBlock = d.getBlock(caller.position);
		var adjBlock = d.getAdjBlock(caller.position, dir);

		if(adjBlock != null && currentBlock.passable[dir]) {
			if(d.getEntity(caller.position + dir) == null) {
				return true;
			}
		}

		return false;
	}

	public override void onValid(Dungeon d) {
		caller.position += dir;
	}

	public override void onInvalid(Dungeon d) { }
}
