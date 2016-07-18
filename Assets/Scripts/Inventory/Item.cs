using UnityEngine;
using System.Collections.Generic;

// Individual items
public class Item  {

    // Universal components
    public int ID = -1;
    public string Title = "";
    public int Value = -1;
    public int CharSlot = -1;
    public string ItemType = "";
	public string Description = "";
    public bool Stackable = false;
    public int MaxStack = -1;
    public bool Useable = false;
	public int Rarity = -1;
    public string Slug = "";
    public Sprite Sprite { get; set; }

    // Specific components
    public int Power = -1;
    public int Durability = -1;
    public int Strength = -1;
    public int Dexterity = -1;
    public int Magic = -1;
    public int Vitality = -1;
    public int Uses = -1;
    public int HealthRestore = -1;
    public int ManaRestore = -1;
    public int Hardness = -1;
    public int Quality = 1;
    public string MatType = "";

	// Constructor
	public Item (int id, string title, int val, int charSlot, string itemType, string description, bool stackable, int maxStack, bool useable, int rarity, string slug) {

		this.ID = id;
		this.Title = title;
		this.Value = val;
        this.CharSlot = charSlot;
        this.ItemType = itemType;
		this.Description = description;
		this.Stackable = stackable;
        this.MaxStack = maxStack;
        this.Useable = useable;
		this.Rarity = rarity;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
	}

	// Create emtpy item
	public Item () {
		this.ID = -1;
	}
}