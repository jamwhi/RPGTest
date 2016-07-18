using UnityEngine;
using System.Collections.Generic;

// Individual items
public class Item  {

    // Universal components
    public int id = -1;
    public string title = "";
    public int value = -1;
    public int charSlot = -1;
    public string itemType = "";
	public string description = "";
    public bool stackable = false;
    public int maxStack = -1;
    public bool useable = false;
	public int rarity = -1;
    public string slug = "";
    public Sprite sprite { get; set; }

    // Specific components
    public int power = -1;
    public int durability = -1;
    public int strength = -1;
    public int dexterity = -1;
    public int magic = -1;
    public int vitality = -1;
    public int uses = -1;
    public int healthRestore = -1;
    public int manaRestore = -1;
    public int hardness = -1;
    public int quality = 1;
    public string matType = "";

	public void SetSprite () {
		this.sprite = Resources.Load<Sprite>("Sprites/Items/" + this.slug);
	}
}