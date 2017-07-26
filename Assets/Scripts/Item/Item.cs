using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item")]
public class Item : ScriptableObject {
	public Texture2D objectTexture;
	public string publicName;
	public int rarity;
	public bool usable;

}