using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Recipe {

    public int id = -1;
    public string title = "";
    public string itemType = "";
    public string tool = "";
    public int metal = -1;
    public int wood = -1;
    public int leather = -1;
    public int diamond = -1;
    public int plating = -1;
    public int blade = -1;
    public int hilt = -1;
    public string description = "";
    public string slug = "";
    public Sprite sprite;

    public void SetSprite() {
        this.sprite = Resources.Load<Sprite>("Sprites/Recipes/" + this.slug);
    }

}
