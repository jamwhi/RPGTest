using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public abstract class Slot : MonoBehaviour,
    IDropHandler, 
    IPointerClickHandler {

    public int slotID;
    public ItemData item;
    public MouseController mouseController;
    public AudioSource audioSource;
    public AudioClip itemDown;

	// Use this for initialization
	protected virtual void Awake () {
        this.item = null;
        GameObject UI = GameObject.FindWithTag("UI");
        mouseController = UI.GetComponent<MouseController>();
        audioSource = UI.GetComponent<AudioSource>();
    }

    public virtual void OnPointerClick(PointerEventData eventData) {
        mouseController.ClickOnSlot(this);
        audioSource.PlayOneShot(itemDown);
    }

    public virtual void OnDrop(PointerEventData eventData) {
        if (mouseController.itemObjOnMouse != null) {
            mouseController.DropItemToSlot<Slot>(this);
        }
    }

}
