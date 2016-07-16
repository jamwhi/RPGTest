using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class Slot : MonoBehaviour,
    IDropHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler {

    public int slotID;
    public Image myColor;
    public Color selectedColor;
    public Color unselectedColor;

    public Inventory owner;
    public ItemData item;
    public Stack stack;
    public Transaction transController;
    public MenuController menuController;
    public MouseController mouseController;
    public AudioSource audioController;
    public AudioClip itemDown;
    public AudioClip itemUp;

    public bool isSelected = false;

    // Use this for initialization
    protected void Awake () {
        this.item = null;
        GameObject UI = GameObject.FindWithTag("UI");
        mouseController = UI.GetComponent<MouseController>();
        audioController = UI.GetComponent<AudioSource>();
        stack = UI.GetComponent<Stack>();
        transController = UI.GetComponent<Transaction>();
        menuController = UI.GetComponent<MenuController>();
        //myColor = gameObject.GetComponent<Image>().color;
    }

    public void SelectSlot() {
        if (this.owner.invType != 2) {
            if (isSelected) {
                myColor.color = unselectedColor;
                this.isSelected = false;
            } else {
                myColor.color = selectedColor;
                this.isSelected = true;
            }
        }
    }

    public void Attach(ItemData itemIn) {
        itemIn.transform.SetParent(this.transform);
        itemIn.slot = this;
        itemIn.owner = this.owner;
        this.item = itemIn;
        item.transform.localPosition = Vector2.zero;
    }

    public bool CombineItems(ItemData itemFromMouse, ItemData itemInSlot) {
        // Check if items are the same type, and stackable
        if (itemFromMouse.item.Stackable && (itemFromMouse.item.ID == itemInSlot.item.ID)) {
            int totalAmount = itemFromMouse.amount + itemInSlot.amount;
            if (itemInSlot.amount == itemInSlot.item.MaxStack) {
                return true;
            } else if (totalAmount <= itemInSlot.item.MaxStack) {
                Debug.Log("Adding stacks together, total: " + totalAmount.ToString());
                itemInSlot.SetAmount(totalAmount);
                Destroy(mouseController.itemObjOnMouse);
                mouseController.RemoveItem();
                return false;
            } else if (totalAmount > itemInSlot.item.MaxStack) {
                itemInSlot.SetAmount(itemInSlot.item.MaxStack);
                mouseController.itemDataOnMouse.SetAmount(totalAmount - itemInSlot.amount);
                return false;
            }

            Debug.Log("Should not be here. (CombineItems function)");
        }
        return true;
    }

    // Determine on click behaviour
    public void OnPointerClick(PointerEventData eventData) {
        this.owner.transform.SetSiblingIndex(2);
        // On Left click
        if (eventData.button == PointerEventData.InputButton.Left) {
            // IF mouse is empty
            if (mouseController.itemObjOnMouse == null) {
                // IF slot is empty
                if (item == null) {
                    return;
                }
                // ELSE slot is full
                else {
                    // IF shop is open, select slot
                    if (menuController.shop.activeSelf && this.owner.invType != 2) {
                        this.SelectSlot();
                        return;
                    }
                    // IF shift is down, open stack
                    if (Input.GetKey(KeyCode.LeftShift) && item.item.Stackable) {
                        stack.Activate(this.item, (Vector2)eventData.position);
                    }
                    // ELSE shift is not down, attach to mouse
                    else {
                        mouseController.AttachItemToMouse(this.item.gameObject);
                        this.item = null;
                        audioController.PlayOneShot(itemUp);
                    }
                }
            }
            // ELSE mouse is full
            else {
                // IF no transaction occurs
                if (transController.ItemTransaction(mouseController.itemDataOnMouse, this.owner, this.slotID)) {
                    // IF slot is empty, drop item into slot
                    if (item == null) {
                        this.Attach(mouseController.itemDataOnMouse);
                        mouseController.RemoveItem();
                        audioController.PlayOneShot(itemDown);
                    }
                    // ELSE slot is full, combine or switch items
                    else if (CombineItems(mouseController.itemDataOnMouse, this.item)) {
                        ItemData temp = this.item;
                        temp.slot = mouseController.itemDataOnMouse.slot;
                        Attach(mouseController.itemDataOnMouse);
                        mouseController.AttachItemToMouse(temp.gameObject);
                        //mouseController.SwapItems(this.item, mouseController.itemDataOnMouse);
                        audioController.PlayOneShot(itemUp);
                    }
                }
            }
        }
        // On right click 
        else {
            // do nothing :)
        }
    }

    // ----------------Drag and Drop items---------------------

    // Begin
    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Left) {
            if (this.item != null) {
                if (this.isSelected) {
                    SelectSlot();
                }
                // IF item on mouse
                if (mouseController.itemObjOnMouse != null) {
                    ItemData temp = this.item;
                    temp.slot = mouseController.itemDataOnMouse.slot;
                    this.Attach(mouseController.itemDataOnMouse);
                    mouseController.AttachItemToMouse(temp.gameObject);
                    return;
                }
                // ELSE mouse is empty
                else {
                    mouseController.AttachItemToMouse(this.item.gameObject);
                    this.item = null;
                    audioController.PlayOneShot(itemUp);
                }
            }
        }
    }

    // Updating item position occurs in MouseController
    // This function still needs to be here because reasons
    public void OnDrag(PointerEventData eventData) {
    }

    // End
    public void OnEndDrag(PointerEventData eventData) {
        // This ensures the item locks to a slot if it is released 
        // not over a slot. If the item is dropped over a slot,
        // this function essentially fires twice (once in OnDrop)
        // Maybe there is a better way.
        if (this.mouseController.itemObjOnMouse != null) {
            mouseController.itemDataOnMouse.slot.Attach(mouseController.itemDataOnMouse);
            mouseController.RemoveItem();
        }
    }

    public void OnDrop(PointerEventData eventData) {
        if (mouseController.itemObjOnMouse != null) {
            if (this.isSelected) {
                SelectSlot();
            }
            if (transController.ItemTransaction(mouseController.itemDataOnMouse, this.owner, this.slotID)) {
                //IF this slot is full, swap item slots
                if (this.item != null) {
                    if (CombineItems(mouseController.itemDataOnMouse, this.item)) {
                        ItemData tempData = this.item;
                        Slot tempSlot = mouseController.itemDataOnMouse.slot;
                        Attach(mouseController.itemDataOnMouse);
                        tempSlot.Attach(tempData);
                        mouseController.RemoveItem();
                    }
                    audioController.PlayOneShot(itemDown);
                }
            // ELSE this slot is empty
                else {
                        // IF no transaction occurs
                        if (transController.ItemTransaction(mouseController.itemDataOnMouse, this.owner, this.slotID)) {
                            this.Attach(mouseController.itemDataOnMouse);
                            audioController.PlayOneShot(itemDown);
                        }
                        mouseController.RemoveItem();
                }
            }
        }
    }
    // ----------------End Drag Handling-----------------------

    // ------------------Tooltip handing-----------------------
    public void OnPointerEnter(PointerEventData eventData) {
        if (!stack.isActive) {
            if (this.item != null) {
                mouseController.ActivateTooltip(this.item.item, eventData.position);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseController.DeactivateTooltip();
    }
    // ---------------End Tooltip handing----------------------


}
