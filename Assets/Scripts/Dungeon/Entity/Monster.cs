using UnityEngine;

/// <summary>Monster is an entity, that can hurt player. It's build from <c>MonsterTemplate</c>.</summary>
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Monster : Entity {
	/// <summary>Try to load <c>MonsterTemplate</c> from available resources.</summary>
	public static MonsterTemplate GetTemplate(string id) {
		var template = Resources.Load<MonsterTemplate>("Entities/" + id);

		if(template == null) {
			Debug.LogError("Monster '" + id + "' doesn't exist");
		}

		return template;
	}

	/// <summary>Set template, initialize proerties and model from template data.</summary>
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