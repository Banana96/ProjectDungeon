using UnityEngine;

public abstract class ItemTemplate : ScriptableObject {
	public string publicName;
	public Stacking stacking;
	public Rarity rarity;
	public Texture2D texture;
	public bool special;
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class WeaponTemplate : ItemTemplate {
	public int baseDamage;
	public int bonusDamage;
	public WeaponType type;

	public int getAttackFromRange(System.Random rng) {
		return baseDamage + rng.Next(bonusDamage);
	}
}

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory/Armor")]
public class ArmorTemplate : ItemTemplate {
	public ArmorType type;
	public ArmorPart part;
	public string setName;
	public int defense;
}