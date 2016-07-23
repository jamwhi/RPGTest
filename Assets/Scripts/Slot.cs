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

    public AssetManager assetManager;
    public Inventory owner;
    public ItemData item;
    public Stack stack;
    public Transaction transController;
    public MouseController mouseController;
    public AudioController audioController;
    public AudioClip itemDown;
    public AudioClip itemUp;

    public bool isSelected = false;

    // Use this for initialization
    protected void Awake() {
        this.item = null;
        assetManager = GameObject.FindWithTag("Manager").GetComponent<AssetManager>();
        mouseController = assetManager.mouseController;
        audioController = assetManager.audioController;
        stack = assetManager.stack;
        transController = assetManager.transaction;
    }

    public void SelectSlot() {
        if (this.owner.invType != 2 && this.owner.invType != 3) {
            if (isSelected) {
                myColor.color = unselectedColor;
                this.isSelected = false;
            } else if (this.item != null) {
                myColor.color = selectedColor;
                this.isSelected = true;
            }
        }
    }

    public void Attach(ItemData itemIn) {
        itemIn.transform.SetParent(this.transform);
        itemIn.slot = this;
        itemIn.owner = this.owner;
        item = itemIn;
        item.transform.localPosition = Vector2.zero;
        owner.ItemIntoSlot(this);
        if(owner.invType == 2) {
            Debug.Log("Trying to update character");
            owner.equipment.UpdateEquipment();
        }
        else if(owner.invType == 4) {
            Debug.Log("Dropped item into crafting slot");
            owner.craftingControl.CheckItems();
        }
		audioController.PlaySfx(itemDown);
	}

	public ItemData PickupItem() {
		ItemData it = this.item;
		this.item = null;
        if (owner.invType == 2) {
            Debug.Log("Trying to update character");
            owner.equipment.UpdateEquipment();
        }
        audioController.PlaySfx(itemUp);
		return it;
	}

    public ItemData CombineOrSwap(ItemData i) {
        // Check if items are the same type, and stackable
        if (i.item.stackable && i.item.id == item.item.id) {
			ItemData n = Combine(i);
			if (n != null) {
				return n;
			}
			return null;
        } else {
			Debug.Log("Swapping items.");
			return Swap(i);
		}
    }

	private ItemData Combine(ItemData i) {
		audioController.PlaySfx(itemDown);
		int leftover = item.AddToStack(i.amount);

		if (leftover > 0) {
			Debug.Log("Stack full, returning " + leftover + " leftover items.");
			i.amount = leftover;
			return i;
		}

		return null;
	}

	public ItemData Swap(ItemData itemToSwap) {
		ItemData it = PickupItem();
		Attach(itemToSwap);
		return it;
	}

    // Determine on click behaviour
    public void OnPointerClick(PointerEventData eventData) {
        owner.transform.SetAsLastSibling();
        // On Left click
        if (eventData.button == PointerEventData.InputButton.Left) {
            mouseController.HandleClick(this, eventData.position);    
        }
        // On right click 
        else if (eventData.button == PointerEventData.InputButton.Right) {
            mouseController.HandleRightClick(this);
        }
    }

	// ----------------Drag and Drop items---------------------
	public void OnBeginDrag (PointerEventData eventData) {
        if (item == null) return;
        if (eventData.button == PointerEventData.InputButton.Right) return;
        mouseController.StartDrag(this, eventData.position);
	}

	public void OnDrag (PointerEventData eventData) {}

	public void OnEndDrag (PointerEventData eventData) {
	}

	public void OnDrop (PointerEventData eventData) {
        owner.transform.SetAsLastSibling();
		mouseController.EndDrag(this);
	}

    // ----------------End Drag Handling-----------------------

    // ------------------Tooltip handing-----------------------
    public void OnPointerEnter(PointerEventData eventData) {
        if (!stack.isActive) {
            if (this.item != null) {
                mouseController.ActivateTooltipItem(this.item.item, eventData.position);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseController.DeactivateTooltip();
    }
    // ---------------End Tooltip handing----------------------


}
