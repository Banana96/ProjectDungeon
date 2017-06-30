using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Monster : Entity {
	public static MonsterTemplate GetTemplate(string id) {
		var template = Resources.Load<MonsterTemplate>("Entities/" + id);

		if(template == null) {
			Debug.LogError("Monster '" + id + "' doesn't exist");
		}

		return template;
	}

	public void Build(MonsterTemplate temp) {
		if(temp == null) {
			Debug.LogError("Given null MonsterTemplate");
		} else {
			template = temp;
			currentHealth = template.health;

			var mf = GetComponent<MeshFilter>();
			mf.sharedMesh = temp.mesh;

			var mr = GetComponent<MeshRenderer>();
			mr.sharedMaterial = temp.material;
		}
	}
}