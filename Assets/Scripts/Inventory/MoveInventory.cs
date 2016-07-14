using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class MoveInventory : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler {

    public Transform inventory;

    private Vector2 offset;

    public void OnPointerDown(PointerEventData eventData) {
        offset = eventData.position - (Vector2) inventory.position;
    }

    public void OnDrag(PointerEventData eventData) {
        inventory.position = eventData.position - offset;
    }
}
