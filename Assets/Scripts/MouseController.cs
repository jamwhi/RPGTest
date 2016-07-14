using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseController : MonoBehaviour {

    public Inventory inventory;
    public Tooltip tooltip;
    public Stack stack;
    public GameObject itemObjOnMouse;
    public ItemData itemDataOnMouse;

    void Update() {
        // If an item is on the mouse, move its position.
        if (itemObjOnMouse != null) {
            itemObjOnMouse.transform.position = Input.mousePosition;
        }
    }

    // Attach an item to the mouse
    public void AttachItemToMouse(GameObject item) {

        if (item == null) {
            itemObjOnMouse = null;
            itemDataOnMouse = null;
        }
        if(itemObjOnMouse != null) {
            ItemData tempItemData = item.GetComponent<ItemData>();
            Slot tempSlot = tempItemData.slot;
            tempItemData.slot = itemDataOnMouse.slot;
            AttachItemToSlot(itemObjOnMouse, tempSlot);
            itemObjOnMouse = item;
            itemDataOnMouse = tempItemData;
        } 
        else {
            itemObjOnMouse = item;
            itemDataOnMouse = itemObjOnMouse.GetComponent<ItemData>();
        }
        // Set intial movement states
        itemObjOnMouse.GetComponent<CanvasGroup>().blocksRaycasts = false;
        itemObjOnMouse.transform.SetParent(this.transform);
        itemObjOnMouse.transform.position = Input.mousePosition;
    }

    public void AttachItemToSlot(GameObject item, Slot intoSlot) {

        if (intoSlot == null) {
            Debug.Log("Don't have a valid target slot");
        } 
        else {
            item.GetComponent<ItemData>().slot = intoSlot;
            intoSlot.item = item.GetComponent<ItemData>();
            item.transform.SetParent(intoSlot.transform);
            item.transform.localPosition = new Vector3(32, -32, 0);
            item.GetComponent<CanvasGroup>().blocksRaycasts = true;
            itemObjOnMouse = null;
            itemDataOnMouse = null;
        }
    }

    public void DropItemToSlot(Slot intoSlot) {

        if(intoSlot.owner != itemDataOnMouse.owner) {
            Debug.Log("Diff Owner.");
        }
        if(intoSlot.owner == itemDataOnMouse.owner) {
            Debug.Log("Same Owner");
        }
        // IF no item on mouse during drop (shouldn't occur)
        if (!itemObjOnMouse) {
            Debug.Log("Attempted to drop item in slot, but no item on mouse");
        } 
        // IF item is dropped into its own slot
        else if (intoSlot.item == itemDataOnMouse) {
        }
        // ELSE handle swap
        else {
            DropOntoInventory(intoSlot);
        }
    }

    public void DropOntoInventory(Slot intoSlot) {
        // If the slot is empty
        if (!intoSlot.item) {
            // If the item had a previous slot
            if (itemDataOnMouse.slot != null) {
                itemDataOnMouse.slot.item = null;
            }
            AttachItemToSlot(itemObjOnMouse, intoSlot);
        }
        // If the slot is not empty 
        else {
            if (CombineItems(itemDataOnMouse, intoSlot.item)) {
                GameObject tempItem = intoSlot.item.gameObject;
                Slot prevSlot = itemDataOnMouse.slot;
                itemDataOnMouse.slot = intoSlot;
                tempItem.GetComponent<ItemData>().slot = prevSlot;
                AttachItemToSlot(itemObjOnMouse, intoSlot);
                AttachItemToSlot(tempItem, prevSlot);
            }
        }
    }

    public void ClickOnItem(ItemData clickedItem, Vector2 pos) {
        if (Input.GetKey(KeyCode.LeftShift)
            && (clickedItem.amount > 1)
            && !stack.isActive
            && itemObjOnMouse == null) {
            stack.Activate(clickedItem, pos);
        } 
        else if (itemObjOnMouse == null) {
            AttachItemToMouse(clickedItem.gameObject);
        } 
        else if (itemObjOnMouse != null) {
            if (CombineItems(itemDataOnMouse, clickedItem)) {
                Slot tempSlot = itemDataOnMouse.slot;
                itemDataOnMouse.slot = clickedItem.slot;
                clickedItem.slot = tempSlot;
                AttachItemToSlot(itemObjOnMouse, itemDataOnMouse.slot);
                AttachItemToMouse(clickedItem.gameObject);
            }
        }
    }


    public bool CombineItems(ItemData itemFromMouse, ItemData itemInSlot) {

        if (itemFromMouse.item.Stackable && (itemFromMouse.item.ID == itemInSlot.item.ID)) {
            int totalAmount = itemFromMouse.amount + itemInSlot.amount;
            if (itemInSlot.amount == itemInSlot.item.MaxStack) {
                return true;
            } 
            else if (totalAmount <= itemInSlot.item.MaxStack) {
                Debug.Log("Adding stacks together, total: " + totalAmount.ToString());
                itemInSlot.SetAmount(totalAmount);
                Destroy(itemObjOnMouse);
                itemObjOnMouse = null;
                itemDataOnMouse = null;
                return false;
            } 
            else if (totalAmount > itemInSlot.item.MaxStack) {
                itemInSlot.SetAmount(itemInSlot.item.MaxStack);
                itemDataOnMouse.SetAmount(totalAmount - itemInSlot.amount);
                return false;
            }

        Debug.Log("Should not be here. (CombineItems function)");
        }
        return true;
    }

    public void ClickOnSlot(Slot clickedSlot) {

        if (itemObjOnMouse != null) {
            if (itemDataOnMouse.slot != null) {
                itemDataOnMouse.slot.item = null;
            }
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
