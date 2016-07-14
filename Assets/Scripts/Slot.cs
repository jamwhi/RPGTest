using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Slot : MonoBehaviour,
    IDropHandler, 
    IPointerClickHandler {

    public int slotID;
    public ItemData item;
    public MouseController mouseController;
    public AudioSource audioSource;
    public AudioClip itemDown;

	// Use this for initialization
	protected void Awake () {
        this.item = null;
        GameObject UI = GameObject.FindWithTag("UI");
        mouseController = UI.GetComponent<MouseController>();
        audioSource = UI.GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        mouseController.ClickOnSlot(this);
        audioSource.PlayOneShot(itemDown);
    }

    public void OnDrop(PointerEventData eventData) {
        if (mouseController.itemObjOnMouse != null) {
            mouseController.DropItemToSlot(this);
        }
    }

}
