﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Transaction : MonoBehaviour {

    public GameObject confirmPanel;

    public Inventory shop;
    public Inventory player;

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
        toAdd.goldAmount += amount;
        toAdd.goldDisplay.text = toAdd.goldAmount.ToString();
        toSub.goldAmount -= amount;
        toSub.goldDisplay.text = toSub.goldAmount.ToString();

    }
    public bool ItemTransaction(ItemData itemIn, Inventory invIn, float valueMod = 1.0f) {
        // IF same owner, do nothing
        if (itemIn.owner == invIn) {
            Debug.Log("Same item owner as inventory in Transaction");
            return true;
        }
        // ELSE IF itemIn from character(0), into shop(1)
        else if ((itemIn.owner.invType == 0) && (invIn.invType == 1)) {
            this.updateGold(itemIn.owner, invIn, itemIn.item.Value);
            Destroy(itemIn.gameObject);
            audioController.PlayOneShot(sellItem);
            Debug.Log("Item sold to shop");
            return false;
        }
        // ELSE IF itemIn from shop(1), into character(0)
        else if ((itemIn.owner.invType == 1) && (invIn.invType == 0)) {
            this.updateGold(itemIn.owner, invIn, itemIn.item.Value);
            //audioController.PlayOneShot(sellItem);
            Debug.Log("Item bought by character");
            return true;

        }
        return true;
    }

    public void OpenSaleWindw() {
        Debug.Log("Begin Confirm");

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
                Debug.Log(currSlot.item.item.Title + "   " + currItem.Value.ToString());
                sellString += "<color=" + rarityColors[currItem.Rarity - 1] + ">" + currItem.Title + "</color>\n";
                sellValString += currItem.Value.ToString() + "\n";
                sellTotal += currItem.Value;
            }
        }

        for (int i = 0; i < shop.slots.Count; i++) {
            Slot currSlot = shop.slots[i];
            if (currSlot.isSelected) {
                Item currItem = currSlot.item.item;
                Debug.Log("Shop Slot: " + i.ToString());
                buyString += "<color=" + rarityColors[currItem.Rarity - 1] + ">" + currItem.Title + "</color>\n";
                buyValString += currItem.Value.ToString() + "\n";
                buyTotal -= currItem.Value;
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
                ItemTransaction(currSlot.item, shop);
                currSlot.item = null;
                currSlot.SelectSlot();
            }
        }

        for (int i = 0; i < shop.slots.Count; i++) {
            Slot currSlot = shop.slots[i];
            if (currSlot.isSelected) {           
                ItemTransaction(currSlot.item, player);
                player.AddExistingItem(currSlot.item);
                currSlot.SelectSlot();
            }
        }
        confirmPanel.SetActive(false);
    }
}