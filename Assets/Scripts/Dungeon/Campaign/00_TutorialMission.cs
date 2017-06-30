namespace Campaign {
	public class TutorialMission : DungeonBuilder {
		public TutorialMission(Dungeon d) : base(d) {}

		protected override void Work() {
			var spawnArea = new Area(1, 1, 3, 3);
			var walkArea = new Area(7, 0, 5, 5);
			var chestArea = new Area(15, 1, 3, 3);
			var monsterArea = new Area(15, 8, 5, 3);

			new Composer(dungeon)
				.begin(64, 64)
				.area(spawnArea)
				.player(spawnArea.center, Direction.East)
				.corridor(
					new Position(4, 2),
					new Position(6, 2)
				)
				.area(walkArea)
				.corridor(
					new Position(12, 2),
					new Position(14, 2)
				)
				.area(chestArea)
				.chest(chestArea.center, Direction.East, false)
				.corridor(
					new Position(9, 5),
					new Position(9, 9),
					new Position(14, 9)
				)
				.area(monsterArea)
				.monster("m_dummy", monsterArea.center, Direction.East)
				.finish();
		}
	}
}