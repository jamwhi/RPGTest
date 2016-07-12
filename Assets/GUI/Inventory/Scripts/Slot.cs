using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

//IPointerClickHandler
public class Slot : MonoBehaviour, 
    IDropHandler,
    IPointerClickHandler {

	public int slotId;
    public ItemData slotItemData = null;
    public MouseControl mouseControl;

    private Inventory inventory;

	void Awake(){
		inventory = GameObject.FindWithTag("InventoryPanel").GetComponent<Inventory>();
        mouseControl = inventory.GetComponent<MouseControl>();
    }

    // Triggers when a drag ends over this slot
	public void OnDrop(PointerEventData eventData){
        Debug.Log("slot drop ID: " + slotId.ToString());
        mouseControl.DropItemToSlot(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        mouseControl.ClickOnSlot(this);
    }
    /*
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
*/
}
