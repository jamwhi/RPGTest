using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Recipe {

    public int ID { get; set; }
    public string Title { get; set; }
    public string ItemType { get; set; }
    public Dictionary<string, int> Components;
    public string Description { get; set; }
    public string Slug { get; set; }
    public Sprite Sprite { get; set; }

    public Recipe(int id, string title, string itemType, Dictionary<string, int> components, string slug) {

        this.ID = id;
        this.Title = title;
        this.ItemType = itemType;
        this.Components = components;
        this.Slug = slug;
        this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
    }

    public Recipe() {
        this.ID = -1;
    }
}
