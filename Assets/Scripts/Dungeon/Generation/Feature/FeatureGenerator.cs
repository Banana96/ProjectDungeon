using System;

/// <summary>Abstracted class for generating various parts of a dungeon.</summary>
public abstract class FeatureGenerator {
	public abstract bool generate(Dungeon dungeon, Random rng);
}