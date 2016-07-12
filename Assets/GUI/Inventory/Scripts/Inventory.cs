using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

	private ItemDatabase database;
	private Transform slotPanel;
	private int slotAmount;

	public GameObject inventorySlot;
    public GameObject inventoryItem;
	public List<GameObject> slots = new List<GameObject>();

	void Start () {

        // Load database, define total number of slots
		database = GetComponent<ItemDatabase>();
		slotAmount = 20;
		slotPanel = transform.Find("SlotPanel"); // Returns a Transform

        // Add slots
		for(int i = 0; i < slotAmount; i++){
            GameObject newSlot = Instantiate(inventorySlot);
            Slot newSlotData = newSlot.GetComponent<Slot>();
			newSlot.transform.SetParent(slotPanel);
			newSlot.name = "Slot " + i.ToString();
            newSlotData.slotItemData = null;
            newSlotData.slotId = i;
            slots.Add(newSlot);
        }

		AddItem(0);
		AddItem(1);
		AddItem(2);
		AddItem(1);
		AddItem(0);
        AddItem(3);
        AddItem(4);
        AddItem(5);
        AddItem(5);
        AddItem(5);
        AddItem(5);
        AddItem(5);
        AddItem(5);
        AddItem(5);
        AddItem(5);
    }

// Attempt to add an item to the inventory
	public void AddItem(int id) {

		int ind;
        Item itemToAdd = database.FetchItemByID(id);

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
			if (!currSlot.slotItemData){
                GameObject itemObj = Instantiate(inventoryItem);
                ItemData data = itemObj.GetComponent<ItemData>();
                data.item = itemToAdd;
				data.slot = currSlot;
				itemObj.transform.SetParent(slots[i].transform);
				itemObj.transform.position = Vector2.zero;
				itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
				itemObj.name = itemToAdd.Title;
                currSlot.slotItemData = data;
                return;
			}
		}
        return;
	}

  /*  public GameObject CreateItem() {
     
        GameObject itemObj = Instantiate(inventoryItem);
        ItemData itemData = itemObj.GetComponent<ItemData>();

        return itemObj;
    }
    */
 
    public GameObject CreateItemFromStack(int id, int amount) {

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
	public int SearchInventory(Item item){

		for( int i = 0; i < slotAmount; i++){
            ItemData slotDataToCheck = slots[i].GetComponent<Slot>().slotItemData;
			if ( (slotDataToCheck != null) 
                && (slotDataToCheck.item.ID == item.ID)
                && (slotDataToCheck.amount < item.MaxStack)) {
                return i;
			}
		}
		return -1;

	}
}
