using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public sealed class U {
	public static T RandArrElem<T>(T[] a, Random r) {
		return a[r.Next(a.Length - 1)];
	}

	public static T RandArrElem<T>(List<T> a, Random r) {
		return a[r.Next(a.Count - 1)];
	}

	public static void Reparent(GameObject parent, params GameObject[] children) {
		foreach(var c in children) {
			c.transform.parent = parent.transform;
		}
	}
}