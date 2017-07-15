using UnityEngine;

//TODO
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Portal : Entity {
	public Portal linkedPortal { get; private set; }
}
