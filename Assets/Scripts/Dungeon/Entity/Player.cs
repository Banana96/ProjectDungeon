using UnityEngine;

public class Player : Entity {
	public int keys = 0;

	private void Awake() {
		template = ScriptableObject.CreateInstance<EntityTemplate>();
		template.publicName = "Player";
		template.health = 100;
		template.minAttack = 3;
		template.maxAttack = 6;
		template.invincible = false;
	}

	public void recieveRequest(EntityRequest req) {
		request = req;
	}
}