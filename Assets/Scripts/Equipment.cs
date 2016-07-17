using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Equipment : MonoBehaviour {

    public Inventory equipInventory;
    public Slot headSlot;
    public Slot chestSlot;
    public Slot feetSlot;
    public Slot quickSlot;
    public Slot mainHandSlot;
    public Slot offHandSlot;

    public Text fullStatText;
    public Text itemStatText;
    //public Text attackText;
    //public Text defenseText;
    //public Text healthText;
    //public Text magicText;

    private int[] itemStats = { 0, 0, 0, 0 };
    private int[] baseStats = {10, 10, 100, 100};

	// Use this for initialization
	void Awake () {
        equipInventory.AddSlot(headSlot);
        equipInventory.AddSlot(chestSlot);
        equipInventory.AddSlot(feetSlot);
        equipInventory.AddSlot(mainHandSlot);
        equipInventory.AddSlot(offHandSlot);
        equipInventory.AddSlot(quickSlot);
        UpdateEquipment();
    }	

    public void UpdateEquipment() {
        // Clear item Stats array
        for(int i = 0; i < 4; i++) {
            itemStats[i] = 0;
        }
        // Calculate stats from items
        for(int i = 0; i < (equipInventory.slots.Count - 1); i++) {
            ItemData currItem = equipInventory.slots[i].item;
            if (currItem != null){
                if ((currItem.item.ItemType == "Weapon") || (currItem.item.ItemType == "Ammo")) {
                    itemStats[0] += equipInventory.slots[i].item.item.Power;
                } 
                else if ((currItem.item.ItemType == "Helmet") 
                    || (currItem.item.ItemType == "Armor")
                    || (currItem.item.ItemType == "Boots")
                    || (currItem.item.ItemType == "Shield")) {
                    itemStats[1] += equipInventory.slots[i].item.item.Power;
                }
            }
        }
        // Write to stats
        UpdateText();
    }

    private void UpdateText() {
        
        fullStatText.text = "";
        itemStatText.text = "";
        for(int i = 0; i < 4; i++) {
            fullStatText.text += (baseStats[i] + itemStats[i]).ToString() + "\n";
            itemStatText.text += "(+" + itemStats[i].ToString() + ")\n";
        }
    }

}
