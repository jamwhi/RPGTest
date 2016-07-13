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
    private AudioSource invAudio;
    private AudioClip itemDown;

    void Awake(){
		inventory = GameObject.FindWithTag("InventoryPanel").GetComponent<Inventory>();
        mouseControl = inventory.GetComponent<MouseControl>();
        invAudio = inventory.GetComponent<AudioSource>();
        itemDown = Resources.Load("Sound/ItemDown") as AudioClip;
    }

    // Triggers when a drag ends over this slot
	public void OnDrop(PointerEventData eventData){
        mouseControl.DropItemToSlot(this);
    }

    public void OnPointerClick(PointerEventData eventData) {
        mouseControl.ClickOnSlot(this);
        invAudio.PlayOneShot(itemDown);
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
