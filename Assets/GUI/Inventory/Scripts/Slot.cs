using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

	public int slotId;
    public int itemId = -1;

    private Inventory inventory;

	void Start(){
		inventory = GameObject.FindWithTag("InventoryPanel").GetComponent<Inventory>();
	}

	public void OnDrop(PointerEventData eventData){

		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();

		if(itemId == -1){
            Slot prevSlot = inventory.slots[droppedItem.slot].GetComponent<Slot>();
            prevSlot.itemId = -1;

            itemId = droppedItem.item.ID;
			droppedItem.slot = slotId;
		}
	}

}
