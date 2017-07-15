using UnityEngine;

/// <summary>Template for creating multiple <c>Entity</c> instances.</summary>
[CreateAssetMenu(fileName = "New Entity", menuName = "Entity/Entity")]
public class EntityTemplate : ScriptableObject {
	public string publicName;
	public int health;
	public int level;
	public int minAttack;
	public int maxAttack;
	public bool invincible;
}
