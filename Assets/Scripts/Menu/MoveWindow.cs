using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class MoveWindow : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler {

    public Transform window;

    private Vector2 offset;

    public void OnPointerDown(PointerEventData eventData) {
        this.transform.parent.SetSiblingIndex(2);
        offset = eventData.position - (Vector2) window.position;
    }

    public void OnDrag(PointerEventData eventData) {
        window.position = eventData.position - offset;
    }
}