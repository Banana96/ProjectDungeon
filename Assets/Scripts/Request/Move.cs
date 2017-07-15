public class Move : EntityRequest {
	private Direction direction => target as Direction;

	public Move(Entity e, Direction dir = null) : base(e, dir) {
		if(dir == null) {
			target = e.facing;
		}
	}

	public override bool validateRequest(Dungeon d) {
		var currentBlock = d.getBlock(caller.position);
		var adjBlock = d.getAdjBlock(caller.position, direction);

		if(adjBlock != null && currentBlock.isPassable(direction)) {
			if(d.getEntity(caller.position + direction) == null) {
				return true;
			}
		}

		return false;
	}

	public override void onValid(Dungeon d) {
		caller.position += direction;
	}

	public override void onInvalid(Dungeon d) { }
}
