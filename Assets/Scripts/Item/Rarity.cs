using UnityEngine;

[CreateAssetMenu(fileName = "New Rarity", menuName = "Property/Rarity")]
public sealed class Rarity : ScriptableObject {
	public string Name;
	public int Tier;
	public Color Color;
}