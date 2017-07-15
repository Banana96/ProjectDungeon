using System;

/// <summary>Abstracted class for generating various parts of a dungeon.</summary>
public interface FeatureGenerator {
	bool generate(Dungeon dungeon, Random rng);
}
