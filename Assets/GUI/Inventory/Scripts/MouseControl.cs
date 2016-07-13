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

        if (item == null) {
            itemObjOnMouse = null;
            itemDataOnMouse = null;
        }
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

    public GameObject DropItemToSlot(Slot intoSlot) {

        // IF no item on mouse during drop (shouldn't occur)
        if (!itemObjOnMouse) {
            Debug.Log("Attempted to drop item in slot, but no item on mouse");
            return null;
        } 
        // IF item is dropped into its own slot
        else if (intoSlot.slotItemData == itemDataOnMouse) {
            return null;
        }
        // ELSE handle swap
        else {
            // If the slot is empty
            if (!intoSlot.slotItemData) {
                // If the item had a previous slot
                if (itemDataOnMouse.slot != null) {
                    itemDataOnMouse.slot.slotItemData = null;
                }              
                AttachItemToSlot(itemObjOnMouse, intoSlot);
                return null;
            }
            // If the slot is not empty 
            else {
                bool combineTest = true;
                if (itemDataOnMouse.item.Stackable && (itemDataOnMouse.item.ID == intoSlot.slotItemData.item.ID) ) {
                    combineTest = CombineItems(itemDataOnMouse, intoSlot.slotItemData);
                    return null;
                }
                if (combineTest) {
                    GameObject tempItem = intoSlot.slotItemData.gameObject;
                    Slot prevSlot = itemDataOnMouse.slot;
                    itemDataOnMouse.slot = intoSlot;
                    tempItem.GetComponent<ItemData>().slot = prevSlot;
                    AttachItemToSlot(itemObjOnMouse, intoSlot);
                    AttachItemToSlot(tempItem, prevSlot);
                    return tempItem;
                }
            }
        }
        return null;
    }

    /*
    public void SwapTwoItems(GameObject itemOne, GameObject itemTwo) {

        ItemData itemOneData = itemOne.GetComponent<ItemData>();
        ItemData itemTwoData = itemTwo.GetComponent<ItemData>();
        ItemData tempData = itemOneData;
        itemOneData = itemTwoData;
        itemTwoData = tempData;
    }
    */

    public bool CombineItems(ItemData itemFromMouse, ItemData itemInSlot) {

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

        Debug.Log("Error: End of CombineItems function reached");
        return true;

    }

    public void ClickOnItem(ItemData clickedItem, Vector2 pos) {

        if (Input.GetKey(KeyCode.LeftShift)
            && (clickedItem.amount > 1)
            && !stack.isActive) {
            stack.Activate(clickedItem, pos);
        } 
        else if (itemObjOnMouse == null) {
            AttachItemToMouse(clickedItem.gameObject);
        } 
        else if (itemObjOnMouse != null) {
            bool combineTest = true;
            if (itemDataOnMouse.item.Stackable && (itemDataOnMouse.item.ID == clickedItem.item.ID)) {
                combineTest = CombineItems(itemDataOnMouse, clickedItem);
            }
            if (combineTest) {
                Slot tempSlot = itemDataOnMouse.slot;
                itemDataOnMouse.slot = clickedItem.slot;
                clickedItem.slot = tempSlot;
                AttachItemToSlot(itemObjOnMouse, itemDataOnMouse.slot);
                AttachItemToMouse(clickedItem.gameObject);
            }
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
