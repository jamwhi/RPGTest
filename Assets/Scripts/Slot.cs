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
		audioController.PlayOneShot(itemDown);
	}

	public ItemData PickupItem() {
		ItemData it = this.item;
		this.item = null;
		audioController.PlayOneShot(itemUp);
		return it;
	}

    public ItemData TryCombineItems(ItemData itemToStack) {
        // Check if items are the same type, and stackable
        if (itemToStack.item.Stackable && itemToStack.item.ID == item.item.ID) {

			if (item.amount == item.item.MaxStack) {
				return Swap(itemToStack);
			}

            int totalAmount = itemToStack.amount + item.amount;

			if (totalAmount > item.item.MaxStack) {
				int leftOver = totalAmount - item.item.MaxStack;
				Debug.Log("Stack full, returning " + leftOver + " leftover items.");

				item.SetAmount(item.item.MaxStack);
				itemToStack.SetAmount(leftOver);
				audioController.PlayOneShot(itemDown);

				return itemToStack;
			}

			else {
				Debug.Log("Adding stacks together, total: " + totalAmount.ToString());

				item.SetAmount(totalAmount);
				Destroy(itemToStack.gameObject);

				return null;
			}
        } else {
			Debug.Log("Swapping items.");
			return Swap(itemToStack);
		}
    }

	public ItemData Swap(ItemData itemToSwap) {
		ItemData it = PickupItem();
		Attach(itemToSwap);
		return it;
	}

    // Determine on click behaviour
    public void OnPointerClick(PointerEventData eventData) {
        this.owner.transform.SetSiblingIndex(2);
        // On Left click
        if (eventData.button == PointerEventData.InputButton.Left) {
			mouseController.HandleClick(this, eventData.position);
        }
        // On right click 
        else {
            // do nothing :)
        }
    }

	// ----------------Drag and Drop items---------------------
	public void OnBeginDrag (PointerEventData eventData) { }
	public void OnDrag (PointerEventData eventData) {}
	public void OnEndDrag (PointerEventData eventData) { }
	public void OnDrop (PointerEventData eventData) { }
	/*
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
            mouseController.RetrieveItem();
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
                        mouseController.RetrieveItem();
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
                        mouseController.RetrieveItem();
                }
            }
        }
    }
	*/
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
