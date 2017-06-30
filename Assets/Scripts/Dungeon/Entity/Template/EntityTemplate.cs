using UnityEngine;

[CreateAssetMenu(fileName = "New Entity", menuName = "Entity/Entity")]
public class EntityTemplate : ScriptableObject {
	public string publicName;
	public int health;
	public int level;
	public int minAttack;
	public int maxAttack;
	public bool invincible;
}