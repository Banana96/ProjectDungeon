using System.Collections.Generic;

public class Inventory {
	private List<ItemSlot> slots;

	public int totalSlotCount {
		get { return slots.Count; }
	}

	public int freeSlotCount {
		get {
			var free = 0;

			foreach(ItemSlot t in slots) {
				if(t.empty) {
					free++;
				}
			}

			return free;
		}
	}

	public bool isFull {
		get {
			foreach(ItemSlot t in slots) {
				if(t.empty) {
					return false;
				}
			}

			return true;
		}
	}

	public bool isEmpty {
		get {
			foreach(ItemSlot t in slots) {
				if(!t.empty) {
					return false;
				}
			}

			return true;
		}
	}

	public bool hasFreeSlot {
		get {
			foreach(ItemSlot t in slots) {
				if(t.empty) {
					return true;
				}
			}

			return false;
		}
	}

	public Inventory(int size, params Item[] items) {
		slots = new List<ItemSlot>(size);
		itemIn(items);
	}

	public void itemIn(params Item[] it) {
		if(hasFreeSlot) {
			foreach(var i in it) {
				foreach(var slot in slots) {
					if(slot.empty) {
						slot.put(i);
					}
				}
			}
		}
	}

	public Item itemOut(int index) {
		var item = slots[index].retrieve();
		return item;
	}
}