using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseController : MonoBehaviour {

    public Inventory inventory;
    public Tooltip tooltip;
    public Stack stack;
    public ItemData itemOnMouse = null;
	public MenuController menuController;
	public Transaction transController;

	void Start() {
		GameObject UI = GameObject.FindWithTag("UI");
		menuController = UI.GetComponent<MenuController>();
		transController = UI.GetComponent<Transaction>();
	}

	void Update() {
        // If an item is on the mouse, move its position.
        if (itemOnMouse != null) {
			itemOnMouse.transform.position = Input.mousePosition;
        }
    }


	public void HandleClick(Slot slot, Vector2 pos) {
		// IF mouse is empty
		if (itemOnMouse == null) {
			// IF slot is empty
			if (slot.item == null) {
				return;
			}
			// ELSE slot is full
			else {
				// IF shop is open, select slot
				if (menuController.shop.activeSelf && slot.owner.invType != 2) {
					slot.SelectSlot();
					return;
				}
				// IF shift is down, open stack
				if (Input.GetKey(KeyCode.LeftShift) && slot.item.item.Stackable) {
					stack.Activate(slot.item, pos);
				}
				// ELSE shift is not down, attach to mouse
				else {
					AttachItemToMouse(slot.PickupItem());
				}
			}
		}
		// ELSE mouse is full
		else {
			// IF no transaction occurs
			if (transController.ItemTransaction(itemOnMouse, slot.owner, slot.slotID)) {
				// IF slot is empty, drop item into slot
				if (slot.item == null) {
					slot.Attach(RetrieveItem());
				}
				// ELSE slot is full, combine or switch items
				else {
					ItemData it = slot.CombineOrSwap(itemOnMouse);
					if (it != null) {
						AttachItemToMouse(it);
					} else {
						RetrieveItem().Destroy();
					}
					
				}
			}
		}
	}

    // Clear item references from mouse
    public ItemData RetrieveItem() {
		ItemData it = this.itemOnMouse;
        this.itemOnMouse = null;
		return it;
    }

    // Attach an item to the mouse
    public void AttachItemToMouse(ItemData item) {
		itemOnMouse = item;
		itemOnMouse.transform.SetParent(this.transform);
		itemOnMouse.transform.position = Input.mousePosition;
	}

    public void SwapItems(ItemData itemOne, ItemData itemTwo) {
        Slot tempSlot = itemOne.slot;
        itemOne.slot = itemTwo.slot;
        itemTwo.slot = tempSlot;

    }

    public void ActivateTooltip(Item item, Vector2 pos) {

        if (!itemOnMouse) {
            tooltip.Activate(item, pos);
        }
    }

    public void DeactivateTooltip() {
        tooltip.Deactivate();
    }
}
