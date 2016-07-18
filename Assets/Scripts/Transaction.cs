using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transaction : MonoBehaviour {

    public GameObject confirmPanel;

    public Inventory shop;
    public Inventory player;
    public Inventory equipment;

    public AudioSource audioController;
    public AudioClip sellItem;

    public Text sellItemText;
    public Text sellValText;
    public Text buyItemText;
    public Text buyValText;
    public Text goldText;

    private string[] rarityColors = { "#6A6A6A", "#02BD0B", "#0000FF", "#C200CE" };

    // Update is called once per frame
    void Update() {
    }
    private void updateGold(Inventory toAdd, Inventory toSub, int amount) {
        toAdd.gold.goldAmount += amount;
        toAdd.gold.goldDisplay.text = toAdd.gold.goldAmount.ToString();
        toSub.gold.goldAmount -= amount;
        toSub.gold.goldDisplay.text = toSub.gold.goldAmount.ToString();

    }
    public bool ItemTransaction(ItemData itemIn, Inventory invIn, int slotIndex, float valueMod = 1.0f) {
        // Return if no itemIn (bad call to function)
        if (itemIn == null) {
			return false;
		}
        // IF same owner, do nothing
        if ((itemIn.owner == invIn) && (invIn.invType != 2)) {
            Debug.Log("Same item owner as inventory in Transaction");
            return true;
        }
        // ELSE IF Moving an item from inventory or equipment to shop
        else if (((itemIn.owner.invType == 0) || (itemIn.owner.invType == 2)) && (invIn.invType == 1)) {
            //Chcek if sufficient gold
            if (invIn.gold.goldAmount >= itemIn.item.value) {
                this.updateGold(itemIn.owner, invIn, itemIn.item.value);
                Destroy(itemIn.gameObject);
                audioController.PlayOneShot(sellItem);
                Debug.Log("Item sold to shop");             
            }
            return false;
        }
        // ELSE IF: Moving an item from shop to inventory or equipment
        else if ((itemIn.owner.invType == 1) && ((invIn.invType == 2) || (invIn.invType == 0)) ) {
            //Chcek if sufficient gold
            if (invIn.gold.goldAmount >= itemIn.item.value) {
                this.updateGold(itemIn.owner, invIn, itemIn.item.value);
                //audioController.PlayOneShot(sellItem); Buy item sound?
                Debug.Log("Item bought by character");
                return true;
            } else return false;

        }
        // ELSE IF: Attempting to equip item to wrong equipment slot
        else if( ((itemIn.owner.invType == 0) || (itemIn.owner.invType == 2)) && (invIn.invType == 2) ) {
            if (itemIn.item.charSlot != slotIndex) {
                Debug.Log("Invalid slot");
                return false;
            }
        }
        return true;
    }

    public void OpenSaleWindw() {

        int goldExchanged = 0;
        int sellTotal = 0;
        int buyTotal = 0;
        string sellString = "Selling\n";
        string buyString = "Buying\n";
        string buyValString = "";
        string sellValString = "";

        for (int i = 0; i < player.slots.Count; i++) {
            Slot currSlot = player.slots[i];
            if (currSlot.isSelected) {
                Item currItem = currSlot.item.item;
                Debug.Log(currSlot.item.item.title + "   " + currItem.value.ToString());
                sellString += "<color=" + rarityColors[currItem.rarity - 1] + ">" + currItem.title + "</color>\n";
                sellValString += currItem.value.ToString() + "\n";
                sellTotal += currItem.value;
            }
        }

        for (int i = 0; i < shop.slots.Count; i++) {
            Slot currSlot = shop.slots[i];
            if (currSlot.isSelected) {
                Item currItem = currSlot.item.item;
                Debug.Log("Shop Slot: " + i.ToString());
                buyString += "<color=" + rarityColors[currItem.rarity - 1] + ">" + currItem.title + "</color>\n";
                buyValString += currItem.value.ToString() + "\n";
                buyTotal -= currItem.value;
            }
        }

        goldExchanged = sellTotal + buyTotal;
        buyValString = buyTotal.ToString() + "\n" + buyValString;
        sellValString = sellTotal.ToString() + "\n" + sellValString;
        buyValText.text = buyValString;
        sellValText.text = sellValString;
        sellItemText.text = sellString;
        buyItemText.text = buyString;
        goldText.text = "Gold Exchanged: <color=#D97F1E>" + goldExchanged.ToString() + "</color>";
        confirmPanel.SetActive(true);
    }

    public void SaleConfirm() {

        for (int i = 0; i < player.slots.Count; i++) {
            Slot currSlot = player.slots[i];
            if (currSlot.isSelected) {
                ItemTransaction(currSlot.item, shop, i);
                currSlot.item = null;
                currSlot.SelectSlot();
            }
        }

        for (int i = 0; i < shop.slots.Count; i++) {
            Slot currSlot = shop.slots[i];
            if (currSlot.isSelected) {           
                ItemTransaction(currSlot.item, player, i);
                player.AddExistingItem(currSlot.item);
                currSlot.SelectSlot();
            }
        }
        confirmPanel.SetActive(false);
    }

    public void StackCancel() {
        confirmPanel.SetActive(false);
    }
}
