using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour, IPointerClickHandler {

    public ItemDatabase database;
    public Slot inventorySlot;
    public ItemData inventoryItem;
    public Transform slotPanel;
    public Gold gold;
    public int slotAmount;
    public int invType; // 0 is character, 1 is shop, 2 is equipment

	public List<Slot> slots = new List<Slot>();
    
	protected void Start () {
        // Add slots
		for(int i = 0; i < slotAmount; i++){
            Slot newSlot = Instantiate(inventorySlot) as Slot;
			newSlot.transform.SetParent(slotPanel);
            newSlot.owner = this;
			newSlot.name = "Slot " + i.ToString();
            newSlot.slotID = i;
            slots.Add(newSlot);
        }
    }

// Attempt to add an item to the inventory
	public void AddItem(Item itemToAdd) {

        int ind;

        // If the item ID is invalid
        if (itemToAdd.ID == -1){
			return;
		}

        // If the item is stackable and exists in inventory
		if (itemToAdd.Stackable && (ind = SearchInventory(itemToAdd)) > -1){
			ItemData data = slots[ind].item;
			data.amount++;
			data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
			return;
		}

        // Add new item to inventory
		for(int i = 0; i < slotAmount; i++){
            Slot currSlot = slots[i];
			if (currSlot.item == null){
                ItemData newItem = Instantiate(inventoryItem) as ItemData;
                newItem.item = itemToAdd;
                newItem.slot = currSlot;
                newItem.owner = this;
                newItem.transform.SetParent(slots[i].transform);
                newItem.transform.localPosition = Vector2.zero;
                newItem.GetComponent<Image>().sprite = itemToAdd.Sprite;
                newItem.name = itemToAdd.Title;
                currSlot.item = newItem;
                return;
			}
		}
        return;
	}

    public void AddItemButton(string ind) {

        AddItem(database.FetchItemByID(int.Parse(ind)) );
    }

    public void AddExistingItem(ItemData itemDataToAdd) {

        for (int i = 0; i < slotAmount; i++) {
            Slot currSlot = slots[i];
            if (currSlot.item == null) {
                itemDataToAdd.slot.item = null;
                itemDataToAdd.slot = currSlot;
                itemDataToAdd.owner = this;
                currSlot.Attach(itemDataToAdd);
                return;
            }
        }
    }
 
    public ItemData CreateItemFromStack(int id, int amount) {

        ItemData itemData = Instantiate(inventoryItem) as ItemData;
        Item item = database.FetchItemByID(id);
        itemData.item = item;
        itemData.amount = amount;
        itemData.GetComponent<Image>().sprite = item.Sprite;
        itemData.name = item.Title;

        return itemData;
    }
    

// Search inventory for an item, return index.
	public int SearchInventory(Item item){

		for( int i = 0; i < slotAmount; i++){
            ItemData slotDataToCheck = slots[i].item;
			if ( (slotDataToCheck != null) 
                && (slotDataToCheck.item.ID == item.ID)
                && (slotDataToCheck.amount < item.MaxStack)) {
                return i;
			}
		}
		return -1;
	}

    public void DeselectAll() {
        foreach (Slot currSlot in slots) {
            if (currSlot.isSelected) {
                currSlot.SelectSlot();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        this.transform.SetSiblingIndex(2);
    }
}
