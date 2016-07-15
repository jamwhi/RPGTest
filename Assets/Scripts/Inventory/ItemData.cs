using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ItemData : MonoBehaviour {
	
	public int amount = 1;
    public Slot slot = null;

    public Item item;
	public Inventory owner;

    void Awake() {

        GameObject inv = GameObject.FindWithTag("InventoryPanel");
        GameObject UI = GameObject.FindWithTag("UI");
        owner = inv.GetComponent<Inventory>();
    }

    public void SetAmount(int newAmount) {
        this.amount = newAmount;
        this.gameObject.transform.GetComponentInChildren<Text>().text = this.amount.ToString();
    }
}
