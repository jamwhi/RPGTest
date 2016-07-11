using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {

	public Item item;
	public int amount = 1;
	public int slot = -1;

    private GameObject inventory;
	private Inventory inv;
    private Tooltip tooltip;

	void Start(){
        inventory = GameObject.FindWithTag("InventoryPanel");
		inv = inventory.GetComponent<Inventory>();
        tooltip = inventory.GetComponent<Tooltip>();
        if (tooltip == null) Debug.Log("Unable to find tooltip");
    }

	public void OnBeginDrag(PointerEventData eventData){

		if(item != null){
			this.transform.SetParent(this.inventory.transform);
			this.transform.position = eventData.position;
			GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}

	public void OnDrag(PointerEventData eventData){
		if(item != null){
			this.transform.position = eventData.position;
		}
	}

	public void OnEndDrag(PointerEventData eventData){
		this.transform.SetParent(inv.slots[slot].transform);
		this.transform.localPosition = new Vector3(32,-32,0);
		GetComponent<CanvasGroup>().blocksRaycasts = true;

	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.Activate(item, eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.Deactivate();
    }
}
