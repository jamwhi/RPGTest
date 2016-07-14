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

    public Item item;
    public MouseController mouseControl;
	public Inventory owner;

    private Stack stack;
    private AudioSource audioControl;
    private AudioClip itemUp;
    private AudioClip itemDown;

    void Awake() {

        GameObject inv = GameObject.FindWithTag("InventoryPanel");
        GameObject UI = GameObject.FindWithTag("UI");
        owner = inv.GetComponent<Inventory>();
        mouseControl = UI.GetComponent<MouseController>();
        audioControl = UI.GetComponent<AudioSource>();
        stack = inv.GetComponent<Stack>();
        itemUp = Resources.Load("Sound/ItemUp") as AudioClip;
        itemDown = Resources.Load("Sound/ItemDown") as AudioClip;
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
        audioControl.PlayOneShot(itemUp);
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
            audioControl.PlayOneShot(itemDown);
        } 
	}

    // ----------------End Drag Handling-----------------------

    // ------------------Tooltip handing-----------------------
    public void OnPointerEnter(PointerEventData eventData) {
        if (!stack.isActive) {
            mouseControl.ActivateTooltip(item, eventData.position);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseControl.DeactivateTooltip();
    }
    // ---------------End Tooltip handing----------------------


    // Single Click (left click to attach item to mouse, shift click to open stack)
    public void OnPointerClick(PointerEventData eventData) {
        mouseControl.ClickOnItem(this, eventData.position);
        audioControl.PlayOneShot(itemUp);
    }

}
