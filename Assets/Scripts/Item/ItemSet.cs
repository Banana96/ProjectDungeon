using UnityEngine;

[CreateAssetMenu(fileName = "New Item Set", menuName = "Inventory/Item Set")]
public class ItemSet : ScriptableObject {
	public ItemTemplate[] setItems;
	//public StatModifier[] setBonus;
}
