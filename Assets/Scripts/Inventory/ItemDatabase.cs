using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    // Database of items
	private List<Item> database = new List<Item>();
	private string JString;

	void Start(){

		JString = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json")).ToJson();
		Item[] items = JsonMapper.ToObject<Item[]>(JString);

		for (int i = 0; i < items.Length; i++) {
			items[i].SetSprite();
			database.Add(items[i]);
		}
	}

	public Item FetchItemByID(int id) {

		for(int i = 0; i < database.Count; i++) {
			if(database[i].id == id) {
				return database[i];
			}
		}
		return new Item();
	}
}