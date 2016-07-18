using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class ItemData : MonoBehaviour {
	
	private int Amount = 1;
    public Slot slot = null;

    public Item item;
	public Inventory owner;

    void Awake() {

        GameObject inv = GameObject.FindWithTag("InventoryPanel");
        owner = inv.GetComponent<Inventory>();
    }

	public int amount {
		get { return Amount; }
		set {
			this.Amount = value;
			this.gameObject.transform.GetComponentInChildren<Text>().text = this.amount.ToString();
		}
	}

	public int AddToStack(int n) {

		if (amount >= item.maxStack) {
			// Swap amounts
			Debug.Log("Swapping amounts");
			amount = n;
			return item.maxStack;
		} else {
			// Fill current stack and return leftover
			amount += n;
			if (amount > item.maxStack) {
				Debug.Log("Stack full, returning leftover");
				int overflow = amount - item.maxStack;
				amount = item.maxStack;
				return overflow;
			} else {
				return 0;
			}
		}
	}

    public void Consume() {
        if (item.useable) {
            amount--;
            if (amount == 0) Destroy();
        }

    }

	public void Destroy() {
		Destroy(gameObject);
	}
}
