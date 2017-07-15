public abstract class EntityRequest {
	protected Entity caller { get; private set; }
	protected object target { get; set; }

	public bool isCallerPlayer => caller is Player;

	protected EntityRequest(Entity e, object dir = null) {
		caller = e;
		target = dir;
	}

	public abstract bool validateRequest(Dungeon d);
	public abstract void onValid(Dungeon d);
	public abstract void onInvalid(Dungeon d);
}
