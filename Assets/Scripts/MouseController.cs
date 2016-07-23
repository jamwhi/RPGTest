using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;
using System;

public class MouseController : MonoBehaviour, IPointerClickHandler {

    public Inventory inventory;
    public Tooltip tooltip;
    public Stack stack;
    public GameObject dropItem;
    public Transform frontUI;
    public ItemData itemOnMouse = null;
	public MenuController menuController;
	public Transaction transController;
    public GameObject blockPrefab;
    public GameObject frontBlock;
    public GameObject backBlock;

    private Slot draggingFrom = null;

	public void Awake() {
		GameObject UI = GameObject.FindWithTag("UI");
		menuController = UI.GetComponent<MenuController>();
		transController = UI.GetComponent<Transaction>();
    }

	void Update() {
        // If an item is on the mouse, move its position.
        if (itemOnMouse != null) {
			itemOnMouse.transform.position = Input.mousePosition;
        }    
    }

	public void HandleClick(Slot slot, Vector2 pos) {
        if (stack.gameObject.activeSelf || transController.gameObject.activeSelf) DisableAllFrontLayer();
		// IF mouse is empty
		if (itemOnMouse == null) {
			PickUpFrom(slot, pos);
		}
		// ELSE mouse is full (this must be a clickUP)
		else {
			PutInto(slot);
		}
	}

	public void StartDrag(Slot slot, Vector2 pos) {
		// Check if mouse contains item (this happens if you place an item and begin dragging immediately)
		if (itemOnMouse != null) {

		} else {
			draggingFrom = slot;
			PickUpFrom(slot, pos);
		}
	}
        
	public void EndDrag(Slot slot) {
		if (draggingFrom != null) {
			PutInto(slot);
			draggingFrom = null;
		}
	}
    public void HandleRightClick(Slot slot) {
        if (menuController.shop.activeSelf && slot.owner.invType != 2) {
            slot.SelectSlot();
            return;
        }
        else if (!menuController.shop.activeSelf && !menuController.character.activeSelf) {
            slot.item.Consume();
        }
    }

    public void PickUpFrom(Slot slot, Vector2 pos) {
		// IF slot is empty
		if (slot.item == null) {
			return;
		}
		// ELSE slot is full
		else {
			// IF shop is open, select slot

			// IF shift is down, open stack
			if (Input.GetKey(KeyCode.LeftShift) && slot.item.item.stackable) {
				stack.Activate(slot.item, pos);
			}
			// ELSE shift is not down, attach to mouse
			else {
				AttachItemToMouse(slot.PickupItem());
			}
		}
	}

	public void PutInto(Slot slot) {
		// IF no transaction occurs
		if (transController.ItemTransaction(itemOnMouse, slot.owner, slot.slotID)) {
			// IF slot is empty, drop item into slot
			if (slot.item == null) {
				slot.Attach(RetrieveItem());
			}
			// ELSE slot is full, combine or switch items
			else {
				ItemData it = slot.CombineOrSwap(itemOnMouse);
				if (it != null) {
					if (draggingFrom != null) {
						draggingFrom.Attach(it);
						RetrieveItem();
					} else {
						AttachItemToMouse(it);
					}
				}
				else {
					// Stacks must have combined entirely, since nothing was returned
					RetrieveItem().Destroy();
				}

			}
		}
	}

    public void DropItemConfirm(Vector2 pos) {
        if (itemOnMouse != null) {
            dropItem.SetActive(true);
            dropItem.transform.position = pos;           
        }
    }

    // Drop item from mouse
    public void DropItem() {
        Destroy(RetrieveItem().gameObject);
        dropItem.SetActive(false);
    }

    // Clear item references from mouse
    public ItemData RetrieveItem() {
		ItemData it = this.itemOnMouse;
        this.itemOnMouse = null;
		return it;
    }

    // Attach an item to the mouse
    public void AttachItemToMouse(ItemData item) {
		itemOnMouse = item;
		itemOnMouse.transform.SetParent(this.transform);
		itemOnMouse.transform.position = Input.mousePosition;
	}

    public void ActivateTooltipItem(Item item, Vector2 pos) {
        if (!itemOnMouse) {
            tooltip.ActivateItem(item, pos);
        }
    }

    public void ActivateTooltipRecipe(Recipe recipe, Vector2 pos) {
        if (!itemOnMouse) {
            tooltip.ActivateRecipe(recipe, pos);
        }
    }

    public void DeactivateTooltip() {
        tooltip.Deactivate();
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.pointerEnter.CompareTag("UI")) {
            if (itemOnMouse != null) { 
            DropItemConfirm(eventData.position);
            } 
        }       
    }

    public void CreateBackBlocker() {
        backBlock = Instantiate(blockPrefab);
        backBlock.AddComponent<BackBlocker>();
        backBlock.transform.localPosition = Vector3.zero;
    }

    public void CreateFrontBlocker() {
        frontBlock = Instantiate(blockPrefab);
        frontBlock.AddComponent<FrontBlocker>();
        frontBlock.transform.localPosition = Vector3.zero;
    }

    public void DestroyBackBlocker() {
        Destroy(backBlock.gameObject);
        backBlock = null;
    }

    public void DisableAllFrontLayer() {
        foreach (Transform child in frontUI) {
            child.gameObject.SetActive(false);
        }
        if (frontBlock != null) { 
            Destroy(frontBlock.gameObject);
            frontBlock = null;
        }
    }

}
