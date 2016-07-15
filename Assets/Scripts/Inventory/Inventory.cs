using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour, IPointerClickHandler {

    public ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public Text goldDisplay;
    public Transform slotPanel;
    public int slotAmount;
    public int goldAmount;
    public int invType; // 0 is character, 1 is shop

	public List<GameObject> slots = new List<GameObject>();
    
	protected virtual void Start () {
        // Add slots
		for(int i = 0; i < slotAmount; i++){
            GameObject newSlotObject = Instantiate(inventorySlot);
            Slot newSlot = newSlotObject.GetComponent<Slot>();
			newSlotObject.transform.SetParent(slotPanel);
            newSlot.owner = this;
			newSlot.name = "Slot " + i.ToString();
            newSlot.slotID = i;
            slots.Add(newSlotObject);
        }

		AddItem(database.FetchItemByID(0));
        AddItem(database.FetchItemByID(1));
        AddItem(database.FetchItemByID(2));
        AddItem(database.FetchItemByID(1));
        AddItem(database.FetchItemByID(0));
        AddItem(database.FetchItemByID(3));
        AddItem(database.FetchItemByID(4));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
        AddItem(database.FetchItemByID(5));
    }

// Attempt to add an item to the inventory
	public virtual void AddItem(Item itemToAdd) {

        int ind;

        // If the item ID is invalid
        if (itemToAdd.ID == -1){
			return;
		}

        // If the item is stackable and exists in inventory
		if (itemToAdd.Stackable && (ind = SearchInventory(itemToAdd)) > -1){
			ItemData data = slots[ind].transform.GetChild(0).GetComponent<ItemData>();
			data.amount++;
			data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
			return;
		}

        // Add new item to inventory
		for(int i = 0; i < slotAmount; i++){
            Slot currSlot = slots[i].GetComponent<Slot>();
			if (currSlot.item == null){
                GameObject itemObj = Instantiate(inventoryItem);
                ItemData data = itemObj.GetComponent<ItemData>();
                data.item = itemToAdd;
				data.slot = currSlot;
                data.owner = this;
				itemObj.transform.SetParent(slots[i].transform);
				itemObj.transform.position = Vector2.zero;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.name = itemToAdd.Title;
                currSlot.item = data;
                return;
			}
		}
        return;
	}
 
    public virtual GameObject CreateItemFromStack(int id, int amount) {

        GameObject itemObj = Instantiate(inventoryItem);
        ItemData itemData = itemObj.GetComponent<ItemData>();
        Item item = database.FetchItemByID(id);

        itemData.item = item;
        itemData.SetAmount(amount);
        itemObj.GetComponent<Image>().sprite = item.Sprite;
        itemObj.name = item.Title;

        return itemObj;
    }
    

// Search inventory for an item, return index.
	public virtual int SearchInventory(Item item){

		for( int i = 0; i < slotAmount; i++){
            ItemData slotDataToCheck = slots[i].GetComponent<Slot>().item;
			if ( (slotDataToCheck != null) 
                && (slotDataToCheck.item.ID == item.ID)
                && (slotDataToCheck.amount < item.MaxStack)) {
                return i;
			}
		}
		return -1;
	}

    public void OnPointerClick(PointerEventData eventData) {
        this.transform.SetSiblingIndex(2);
    }
}
