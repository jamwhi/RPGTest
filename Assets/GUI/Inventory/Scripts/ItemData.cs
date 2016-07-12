using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ItemData : MonoBehaviour,
    // Implements 
    IBeginDragHandler, 
    IDragHandler, 
    IEndDragHandler,
    IPointerClickHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler {
	
	public int amount = 1;
    public Slot slot = null;
    public bool onMouse = false;

    public Item item;
    public MouseControl mouseControl;

    private GameObject inventory;
	private Inventory inv;
    private Stack stack;


	void Awake() {

        inventory = GameObject.FindWithTag("InventoryPanel");
		inv = inventory.GetComponent<Inventory>();
        mouseControl = inv.GetComponent<MouseControl>();
        stack = inventory.GetComponent<Stack>();
    }

    void Update() {
 
    }

    public void SetAmount(int newAmount) {
        this.amount = newAmount;
        this.gameObject.transform.GetComponentInChildren<Text>().text = this.amount.ToString();
    }

    // ----------------Drag and Drop items---------------------

    // Begin
    public void OnBeginDrag(PointerEventData eventData) {

            mouseControl.AttachItemToMouse(this.gameObject);
	}

    // During
	public void OnDrag(PointerEventData eventData) {
	}
    // End
	public void OnEndDrag(PointerEventData eventData) {
        // This ensures the item locks to a slot if it is released 
        // not over a slot. If the item is dropped over a slot,
        // this function essentially fires twice (once in OnDrop)
        // Maybe there is a better way.
        if (this.slot != null) {
            mouseControl.AttachItemToSlot(this.gameObject, slot);
        } 
	}

    // ----------------End Drag Handling-----------------------

    // Tooltip on hover 
    public void OnPointerEnter(PointerEventData eventData) {

        if (!stack.isActive) {
            mouseControl.ActivateTooltip(item, eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseControl.DeactivateTooltip();
    }



    // Single Click (left click to attach item to mouse, shift click to open stack)
    public void OnPointerClick(PointerEventData eventData) {

        Debug.Log("Clicked On: " + this.item.Title);
        mouseControl.ClickOnItem(this, eventData.position);

    }

}
