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
    public bool Useable { get; set; }
	public int Rarity { get; set; }
	public string Slug { get; set; }
	public Sprite Sprite { get; set; }

	// Constructor
	public void SetSprite () {
		this.Sprite = Resources.Load<Sprite>("Sprites/Items/" + this.Slug);
	}

	// Create emtpy item
	public Item () {
		this.ID = -1;
	}
}