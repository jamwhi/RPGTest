using UnityEngine;

// Individual items
public class Item {

	public int ID { get; set; }
	public string Title { get; set; }
	public int Value { get; set; }
    public int CharSlot { get; set; }
    public string ItemType { get; set; }
	public int Power { get; set; }
	public int Durability { get; set; }
	public string Description { get; set; }
	public bool Stackable { get; set; }
    public int MaxStack { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

	// Constructor
	public Item (int id, string title, int val, int charSlot, string itemType, int power, int durability, string description, bool stackable, int maxStack, int rarity, string slug) {

		this.ID = id;
		this.Title = title;
		this.Value = val;
        this.CharSlot = charSlot;
        this.ItemType = itemType;
		this.Power = power;
		this.Durability = durability;
		this.Description = description;
		this.Stackable = stackable;
        this.MaxStack = maxStack;
		this.Rarity = rarity;
		this.Slug = slug;
		this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + slug);
	}

	// Create emtpy item
	public Item () {
		this.ID = -1;
	}
}