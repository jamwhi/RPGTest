using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public Item item;
	public int amount = 1;
	public int slot = -1;
    public bool onMouse = false;

    private GameObject inventory;
	private Inventory inv;
    private Tooltip tooltip;
    private Stack stack;


	void Awake(){

        inventory = GameObject.FindWithTag("InventoryPanel");
		inv = inventory.GetComponent<Inventory>();
        tooltip = inventory.GetComponent<Tooltip>();
        stack = inventory.GetComponent<Stack>();

        if (tooltip == null || stack == null)
            Debug.Log("Unable to find a component");
    }

    void Update() {
        if (onMouse) {
            this.transform.position = (Vector2)Input.mousePosition;
        }
 
    }

    // Attach this item to the mouse
    public void AttachToMouse() {

        this.transform.SetParent(this.inventory.transform);
        this.transform.position = Input.mousePosition;
        onMouse = true;
        inv.itemOnMouse = this;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    // Attach this item to its designated slot
    public void AttachToSlot() {

        this.transform.SetParent(inv.slots[slot].transform);
        this.transform.localPosition = new Vector3(32, -32, 0);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        onMouse = false;
    }

    // Swap the item on the mouse for this item
    public void SwapTwoItems(ItemData itemToSwap) {
        int tempSlot = this.slot;
        this.slot = itemToSwap.slot;
        itemToSwap.slot = tempSlot;
        inv.slots[this.slot].GetComponent<Slot>().itemId = this.item.ID;
        inv.slots[itemToSwap.slot].GetComponent<Slot>().itemId = itemToSwap.item.ID;
        this.AttachToSlot();
    }

    // ----------------Drag and Drop items---------------------

    // Begin
    public void OnBeginDrag(PointerEventData eventData) {

		if(item != null){
            AttachToMouse();
        }
	}

    // During
	public void OnDrag(PointerEventData eventData) {

		if(item != null){
			this.transform.position = eventData.position;
		}
	}

    // End
	public void OnEndDrag(PointerEventData eventData) {

        if(item != null) {
            AttachToSlot();
            inv.itemOnMouse.onMouse = false;
            inv.itemOnMouse = null;
        }
	}

    // If item dropped on another item
    public void OnDrop(PointerEventData eventData) {

        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        SwapTwoItems(droppedItem);
    }

    // ----------------End Drag Handling-----------------------

    // Tooltip on hover 
    public void OnPointerEnter(PointerEventData eventData) {

        if (!stack.isActive) {
            tooltip.Activate(item, eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {

        tooltip.Deactivate();
    }

    // Single Click (left click to attach item to mouse, shift click to open stack)
    public void OnPointerClick(PointerEventData eventData) {

        bool shiftDown = Input.GetKey(KeyCode.LeftShift);

        if (shiftDown && item.Stackable && !inv.itemOnMouse && (this.amount > 1)) {
            stack.Activate(this, eventData.pressPosition);
        }
        else if (shiftDown && (this.amount == 1)) {
            return;
        }
        else if (!inv.itemOnMouse) {
            stack.StackCancel();
            AttachToMouse();
        }
        else if (inv.itemOnMouse) {
            SwapTwoItems(inv.itemOnMouse);
            inv.itemOnMouse.AttachToSlot();
            inv.itemOnMouse.onMouse = false;
            inv.itemOnMouse = null;
        }
    }


}
