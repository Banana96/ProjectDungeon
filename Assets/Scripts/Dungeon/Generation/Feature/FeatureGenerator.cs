using System;

public abstract class FeatureGenerator {
	public abstract bool generate(Dungeon dungeon, Random rng);
}