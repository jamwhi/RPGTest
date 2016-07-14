using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public ItemDatabase database;
    public GameObject inventorySlot;
    public GameObject inventoryItem;
    public Text goldDisplay;
    public Transform slotPanel;
    public int slotAmount;
    public int goldAmount;

	public List<GameObject> slots = new List<GameObject>();
    
	protected virtual void Start () {
        // Add slots
		for(int i = 0; i < slotAmount; i++){
            GameObject newSlot = Instantiate(inventorySlot);
            Slot newSlotObject = newSlot.GetComponent<Slot>();
			newSlot.transform.SetParent(slotPanel);
			newSlot.name = "Slot " + i.ToString();
            newSlotObject.slotID = i;
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
	public virtual void AddItem(int id) {

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
            InventorySlot currSlot = slots[i].GetComponent<InventorySlot>();
			if (currSlot.item == null){
                GameObject itemObj = Instantiate(inventoryItem);
                ItemData data = itemObj.GetComponent<ItemData>();
                data.item = itemToAdd;
				data.slot = currSlot;
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
            ItemData slotDataToCheck = slots[i].GetComponent<InventorySlot>().item;
			if ( (slotDataToCheck != null) 
                && (slotDataToCheck.item.ID == item.ID)
                && (slotDataToCheck.amount < item.MaxStack)) {
                return i;
			}
		}
		return -1;

	}
}
