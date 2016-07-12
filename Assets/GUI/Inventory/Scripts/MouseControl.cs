using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseControl : MonoBehaviour {

    public Inventory inventory;
    public Tooltip tooltip;
    public Stack stack;
    public GameObject itemObjOnMouse;
    public ItemData itemDataOnMouse;

    void Awake() {

    }

    void Update() {

        // If an item is on the mouse, move its position.
        if (itemObjOnMouse != null) {
            itemObjOnMouse.transform.position = Input.mousePosition;
        }
    }

    // Attach an item to the mouse
    public void AttachItemToMouse(GameObject item) {

        if(itemObjOnMouse != null) {
            ItemData newItemData = item.GetComponent<ItemData>();
            Slot tempSlot = newItemData.slot;
            newItemData.slot = itemDataOnMouse.slot;
            AttachItemToSlot(itemObjOnMouse, tempSlot);
            itemObjOnMouse = item;
            itemDataOnMouse = newItemData;
        } 
        else {
            itemObjOnMouse = item;
            itemDataOnMouse = itemObjOnMouse.GetComponent<ItemData>();
        }
        // Set intial movement states
        itemObjOnMouse.GetComponent<CanvasGroup>().blocksRaycasts = false;
        itemObjOnMouse.transform.SetParent(inventory.transform);
        itemObjOnMouse.transform.position = Input.mousePosition;
    }

    public void AttachItemToSlot(GameObject item, Slot intoSlot) {

        if (intoSlot == null) {
            Debug.Log("Don't have a valid target slot");
        } 
        else {
            item.GetComponent<ItemData>().slot = intoSlot;
            intoSlot.slotItemData = item.GetComponent<ItemData>();
            item.transform.SetParent(intoSlot.transform);
            item.transform.localPosition = new Vector3(32, -32, 0);
            item.GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemObjOnMouse = null;
            itemDataOnMouse = null;
        }
    }

    public void DropItemToSlot(Slot intoSlot) {

        if (!itemObjOnMouse) {
            Debug.Log("Attempted to drop item in slot, but no item on mouse");
        } 
        // Drop item
        else {
            // If the slot is empty
            if (!intoSlot.slotItemData) {
                // If the item had a previous slot
                if (itemDataOnMouse.slot != null) {
                    itemDataOnMouse.slot.slotItemData = null;
                }              
                AttachItemToSlot(itemObjOnMouse, intoSlot);
            }
            // If the slot is not empty 
            else {
                GameObject tempItem = intoSlot.slotItemData.gameObject;
                Slot prevSlot = itemDataOnMouse.slot;
                itemDataOnMouse.slot = intoSlot;
                tempItem.GetComponent<ItemData>().slot = prevSlot;
                AttachItemToSlot(itemObjOnMouse, intoSlot);
                AttachItemToSlot(tempItem, prevSlot);
            }
        }
    }

    public void ClickOnItem(ItemData clickedItem, Vector2 pos) {

        if (Input.GetKey(KeyCode.LeftShift) && (clickedItem.amount > 1)) {
            stack.Activate(clickedItem, pos);                 
        }
        else if(itemObjOnMouse == null) {
            AttachItemToMouse(clickedItem.gameObject);
        }
        else if(itemObjOnMouse != null) {
            Slot tempSlot = itemDataOnMouse.slot;
            itemDataOnMouse.slot = clickedItem.slot;
            clickedItem.slot = tempSlot;
            AttachItemToSlot(itemObjOnMouse, itemDataOnMouse.slot);
            AttachItemToMouse(clickedItem.gameObject);
        }
    }

    public void ClickOnSlot(Slot clickedSlot) {
        if (itemObjOnMouse != null) {
            AttachItemToSlot(itemObjOnMouse, clickedSlot);
        }
    }

    public void ActivateTooltip(Item item, Vector2 pos) {

        if (!itemObjOnMouse) {
            tooltip.Activate(item, pos);
        }
    }

    public void DeactivateTooltip() {
        tooltip.Deactivate();
    }
}
