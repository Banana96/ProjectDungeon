using UnityEngine;

[CreateAssetMenu(fileName = "newMonster", menuName = "Entity/Monster")]
public class MonsterTemplate : EntityTemplate {
	public BehaviorType behaviorType;
	public Mesh mesh;
	public Material material;
}