using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Blocking : MonoBehaviour, 
    IPointerClickHandler, 
    IDropHandler {

    public MouseController mouseController;
    public delegate void BlockingAction(Vector2 pos);
    public BlockingAction blockingAction;

    // Use this for initialization
    void Awake() {
        mouseController = GameObject.FindGameObjectWithTag("UI").GetComponent<MouseController>();      
        transform.SetParent(mouseController.transform);     
    }

    public void FrontBlocker(Vector2 pos) {
        mouseController.DisableAllFrontLayer();
    }

    public void BackBlocker(Vector2 pos) {
        if(mouseController.itemOnMouse != null) {
            mouseController.DropItemConfirm(pos);
        }
    }

    public void OnDrop(PointerEventData eventData) {
        blockingAction(eventData.position);
    }

    public void OnPointerClick(PointerEventData eventData) {
        blockingAction(eventData.position);
    }

}
