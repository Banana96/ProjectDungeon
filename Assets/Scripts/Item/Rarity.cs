using UnityEngine;

public sealed class Rarity {
	public static readonly Rarity[] All = new Rarity[] {
		new Rarity(0, "Junk",		new Color32(168, 168, 168, 255)),
		new Rarity(1, "Common",		new Color32(255, 255, 255, 255)),
		new Rarity(2, "Rare",		new Color32(0, 204, 0, 255)),
		new Rarity(3, "Magical",	new Color32(255, 204, 102, 255)),
		new Rarity(4, "Legendary",	new Color32(255, 153, 0, 255)),
		new Rarity(5, "Divine",		new Color32(204, 0, 0, 255))
	};

	public static implicit operator int(Rarity r) => r.Num;
	public static implicit operator Rarity(int n) => All[(n % All.Length + All.Length) % All.Length];

	private readonly int num;
	private readonly string publicName;
	private readonly Color itemColor;

	private Rarity(int n, string name, Color col) {
		num = n;
		publicName = name;
		itemColor = col;
	}

	public int Num => num;
	public string Name => publicName;
	public Color Color => itemColor;

}
