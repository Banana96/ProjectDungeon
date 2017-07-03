public class ItemSlot {
	public Item item { get; private set; }
	public bool empty {
		get { return item == null; }
	}

	public ItemSlot(Item it = null, bool mult = false) {
		item = it;
	}

	public void put(Item i) {
		item = i;
	}

	public Item retrieve() {
		var it = item;
		item = null;
		return it;
	}
}