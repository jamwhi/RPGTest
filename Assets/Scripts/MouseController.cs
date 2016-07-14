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

    public void DropItemToSlot<T>(T intoSlot)
        where T : Slot {

        Debug.Log(intoSlot.GetType().Name);
        // IF no item on mouse during drop (shouldn't occur)
        if (!itemObjOnMouse) {
            Debug.Log("Attempted to drop item in slot, but no item on mouse");
        } 
        // IF item is dropped into its own slot
        else if (intoSlot.item == itemDataOnMouse) {
        }
        // ELSE handle swap
        else if (intoSlot is InventorySlot){
            DropOntoInventory(intoSlot as InventorySlot);
        }
        else if (intoSlot is ShopSlot) {
            DropOntoShop(intoSlot as ShopSlot);
        }
    }

    public void DropOntoInventory(InventorySlot intoSlot) {

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
            bool combineTest = true;
            if (itemDataOnMouse.item.Stackable && (itemDataOnMouse.item.ID == intoSlot.item.item.ID)) {
                combineTest = CombineItems(itemDataOnMouse, intoSlot.item);
            }
            if (combineTest) {
                GameObject tempItem = intoSlot.item.gameObject;
                Slot prevSlot = itemDataOnMouse.slot;
                itemDataOnMouse.slot = intoSlot;
                tempItem.GetComponent<ItemData>().slot = prevSlot;
                AttachItemToSlot(itemObjOnMouse, intoSlot);
                AttachItemToSlot(tempItem, prevSlot);
            }
        }

    }

    public void DropOntoShop(ShopSlot intoSlot) {
        inventory.goldAmount += itemDataOnMouse.item.Value;
        inventory.goldDisplay.text = inventory.goldAmount.ToString();
    }

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
