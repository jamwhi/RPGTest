using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler {

	public int slotId;
    public int itemId = -1;

    private Inventory inventory;

	void Start(){
		inventory = GameObject.FindWithTag("InventoryPanel").GetComponent<Inventory>();
	}

    // Triggers when a drag ends over this slot
	public void OnDrop(PointerEventData eventData){

        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        DropItemInSlot(droppedItem);
    }

    public void OnPointerClick(PointerEventData eventData) {

        if (inventory.itemOnMouse) {
            DropItemInSlot(inventory.itemOnMouse);
            inventory.itemOnMouse.AttachToSlot();
            inventory.itemOnMouse.onMouse = false;
            inventory.itemOnMouse = null;
        }
    }

    // Drop item into slot
    private void DropItemInSlot(ItemData droppedItem) {

        if (itemId == -1) {
            if (droppedItem.slot > -1) {
                Slot prevSlot = inventory.slots[droppedItem.slot].GetComponent<Slot>();
                prevSlot.itemId = -1;
            }
            itemId = droppedItem.item.ID;
            droppedItem.slot = slotId;
        }
    }
}
