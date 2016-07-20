using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class RecipeSlot : MonoBehaviour, 
    IPointerClickHandler, 
    IPointerEnterHandler,
    IPointerExitHandler {

    public bool isSelected = false;
    public int recpNum = -1;
    public Color selectedColor;
    public Color unselectedColor;
    public Image myColor;
    public Image image;
    public RecipeSelector owner;
    public Recipe recipe;
    public MouseController mouseController;
    public Stack stack;
    public Transaction transController;

    void Awake() {
        GameObject UI = GameObject.FindWithTag("UI");
        mouseController = UI.GetComponent<MouseController>();
        stack = UI.GetComponent<Stack>();
        transController = UI.GetComponent<Transaction>();
    }

    void Start() {

    }

    public void Select() {
        isSelected = true;
        myColor.color = selectedColor;
        owner.selectedSlot = this;
    }

    public void Deselect() {
        isSelected = false;
        myColor.color = unselectedColor;
    }

    public void SelectSlot() {
        if(owner.selectedSlot != null) {
            owner.selectedSlot.Deselect();
            Select();
        } else {
            Select();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        SelectSlot(); 
    }

    public void OnPointerEnter(PointerEventData eventData) {
           if (!stack.isActive) {
               if (this.recipe != null) {
                   mouseController.ActivateTooltipRecipe(this.recipe, eventData.position);
               }
           }      
    }

    public void OnPointerExit(PointerEventData eventData) {
           mouseController.DeactivateTooltip();
       }

    }
