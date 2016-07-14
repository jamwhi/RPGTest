using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

//IPointerClickHandler
public class InventorySlot : Slot {

    private Inventory inventory;

    protected override void Awake(){
		inventory = GameObject.FindWithTag("InventoryPanel").GetComponent<Inventory>();
        itemDown = Resources.Load("Sound/ItemDown") as AudioClip;
        base.Awake();
    }
}
