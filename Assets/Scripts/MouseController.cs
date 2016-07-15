using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseController : MonoBehaviour {

    public Inventory inventory;
    public Tooltip tooltip;
    public Stack stack;
    public GameObject itemObjOnMouse = null;
    public ItemData itemDataOnMouse = null;

    void Update() {
        // If an item is on the mouse, move its position.
        if (itemObjOnMouse != null) {
            itemObjOnMouse.transform.position = Input.mousePosition;
        }
    }

    // Clear item references from mouse
    public void RemoveItem() {
        this.itemObjOnMouse = null;
        this.itemDataOnMouse = null;
    }

    // Attach an item to the mouse
    public void AttachItemToMouse(GameObject item) {
        itemObjOnMouse = item;
        itemDataOnMouse = itemObjOnMouse.GetComponent<ItemData>();
        itemObjOnMouse.transform.SetParent(this.transform);
        itemObjOnMouse.transform.position = Input.mousePosition;
    }

    public void SwapItems(ItemData itemOne, ItemData itemTwo) {
        Slot tempSlot = itemOne.slot;
        itemOne.slot = itemTwo.slot;
        itemTwo.slot = tempSlot;

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
