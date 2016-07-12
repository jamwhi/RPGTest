using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    // Database of items
	private List<Item> database = new List<Item>();
	private JsonData itemsInDatabase;

	void Start(){

		itemsInDatabase = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
		ConstructItemDatabase();
	}

	public Item FetchItemByID(int id) {

		for(int i = 0; i < database.Count; i++) {
			if(database[i].ID == id) {
				return database[i];
			}
		}
		return new Item();
	}

	private void ConstructItemDatabase() {

		for(int i = 0; i < itemsInDatabase.Count; i++) {
			database.Add(new Item( 
				(int)itemsInDatabase[i]["id"],
                itemsInDatabase[i]["title"].ToString(), 
				(int)itemsInDatabase[i]["val"],
                (int)itemsInDatabase[i]["charSlot"],
                itemsInDatabase[i]["itemType"].ToString(),
				(int)itemsInDatabase[i]["stats"]["power"],
				(int)itemsInDatabase[i]["stats"]["durability"],
                itemsInDatabase[i]["description"].ToString(),
				(bool)itemsInDatabase[i]["stackable"],
				(int)itemsInDatabase[i]["rarity"],
                itemsInDatabase[i]["slug"].ToString()
				));
		}
	}
}