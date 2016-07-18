using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Equipment : MonoBehaviour {

    public Inventory equipInventory;
    public Slot[] equipSlots;
    public string[] statTypes;
    public int[] startStats;
    public Text fullStatText;
    public Text itemStatText;
    //public Text attackText;
    //public Text defenseText;
    //public Text healthText;
    //public Text magicText;

    private int[] itemStats = { 0, 0, 0, 0 };

	// Use this for initialization
	void Awake () {
        foreach(Slot toAdd in equipSlots) {
            equipInventory.AddSlot(toAdd);
        }
        UpdateEquipment();
    }	

    public void UpdateEquipment() {
        // Calculate stats from items
        for(int i = 0; i < 4; i++) {
            itemStats[i] = 0;
        }
        foreach(Slot currSlot in equipSlots){
            if (currSlot.item != null) {
                Item currItem = currSlot.item.item;           
                itemStats[0] += currItem.strength;
                itemStats[1] += currItem.dexterity;
                itemStats[2] += currItem.magic;
                itemStats[3] += currItem.vitality;
            }
        }
        // Write to stats
        UpdateText();
    }

    private void UpdateText() {

        fullStatText.text = "";
        itemStatText.text = "";     
        for(int i = 0; i < 4; i++) {
            fullStatText.text += (startStats[i] +itemStats[i]).ToString() + "\n";
            itemStatText.text += "(+" + itemStats[i].ToString() + ")\n";
        }
    }

}
