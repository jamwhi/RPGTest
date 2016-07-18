using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;

public class ItemDatabase : MonoBehaviour {

    // Database of items
	private List<Item> database = new List<Item>();
	private string JString;

	private JsonData itemsInDatabase;
	private List<Item> databaseFake = new List<Item>();

	void Start () {
		List<Item> db = new List<Item>();
		for (int n = 0; n < 500; n++) {


			/*JString = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json")).ToJson();
			Item[] items = JsonMapper.ToObject<Item[]>(JString);

			for (int i = 0; i < items.Length; i++) {
				items[i].SetSprite();
				db.Add(items[i]);
			}*/

			itemsInDatabase = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Items.json"));
			ConstructItemDatabaseFake();


		}

		for (int i = 0; i < db.Count; i++) {
			ConstructItemDatabase();
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

	private void ConstructItemDatabaseFake () {

		for (int i = 0; i < itemsInDatabase.Count; i++) {

			Item itemIn = new Item(
				(int)itemsInDatabase[i]["id"],
				itemsInDatabase[i]["title"].ToString(),
				(int)itemsInDatabase[i]["value"],
				(int)itemsInDatabase[i]["charSlot"],
				itemsInDatabase[i]["itemType"].ToString(),
				itemsInDatabase[i]["description"].ToString(),
				(bool)itemsInDatabase[i]["stackable"],
				(int)itemsInDatabase[i]["maxStack"],
				(bool)itemsInDatabase[i]["useable"],
				(int)itemsInDatabase[i]["rarity"],
				itemsInDatabase[i]["slug"].ToString()
			);
			switch (itemIn.itemType) {

				case "Weapon":
				case "Armor":
				case "Shield":
					itemIn.power = (int)itemsInDatabase[i]["power"];
					itemIn.strength = (int)itemsInDatabase[i]["strength"];
					itemIn.dexterity = (int)itemsInDatabase[i]["dexterity"];
					itemIn.magic = (int)itemsInDatabase[i]["magic"];
					itemIn.vitality = (int)itemsInDatabase[i]["vitality"];
					itemIn.durability = (int)itemsInDatabase[i]["durability"];
					break;

				case "Consumable":
					itemIn.uses = (int)itemsInDatabase[i]["uses"];
					itemIn.healthRestore = (int)itemsInDatabase[i]["healthRestore"];
					itemIn.manaRestore = (int)itemsInDatabase[i]["manaRestore"];
					break;

				case "Ammo":
					itemIn.power = (int)itemsInDatabase[i]["power"];
					break;

				case "Tool":
					itemIn.hardness = (int)itemsInDatabase[i]["hardness"];
					itemIn.durability = (int)itemsInDatabase[i]["durability"];
					break;

				case "Material":
					itemIn.matType = itemsInDatabase[i]["matType"].ToString();
					itemIn.quality = (int)itemsInDatabase[i]["quality"];
					break;

				default:
					Debug.Log("Unrecognized item in database.");
					break;
			}

			databaseFake.Add(itemIn);
		}
	}

	private void ConstructItemDatabase () {

		for (int i = 0; i < itemsInDatabase.Count; i++) {

			Item itemIn = new Item(
				(int)itemsInDatabase[i]["id"],
				itemsInDatabase[i]["title"].ToString(),
				(int)itemsInDatabase[i]["value"],
				(int)itemsInDatabase[i]["charSlot"],
				itemsInDatabase[i]["itemType"].ToString(),
				itemsInDatabase[i]["description"].ToString(),
				(bool)itemsInDatabase[i]["stackable"],
				(int)itemsInDatabase[i]["maxStack"],
				(bool)itemsInDatabase[i]["useable"],
				(int)itemsInDatabase[i]["rarity"],
				itemsInDatabase[i]["slug"].ToString()
			);
			switch (itemIn.itemType) {

				case "Weapon":
				case "Armor":
				case "Shield":
					itemIn.power = (int)itemsInDatabase[i]["power"];
					itemIn.strength = (int)itemsInDatabase[i]["strength"];
					itemIn.dexterity = (int)itemsInDatabase[i]["dexterity"];
					itemIn.magic = (int)itemsInDatabase[i]["magic"];
					itemIn.vitality = (int)itemsInDatabase[i]["vitality"];
					itemIn.durability = (int)itemsInDatabase[i]["durability"];
					break;

				case "Consumable":
					itemIn.uses = (int)itemsInDatabase[i]["uses"];
					itemIn.healthRestore = (int)itemsInDatabase[i]["healthRestore"];
					itemIn.manaRestore = (int)itemsInDatabase[i]["manaRestore"];
					break;

				case "Ammo":
					itemIn.power = (int)itemsInDatabase[i]["power"];
					break;

				case "Tool":
					itemIn.hardness = (int)itemsInDatabase[i]["hardness"];
					itemIn.durability = (int)itemsInDatabase[i]["durability"];
					break;

				case "Material":
					itemIn.matType = itemsInDatabase[i]["matType"].ToString();
					itemIn.quality = (int)itemsInDatabase[i]["quality"];
					break;

				default:
					Debug.Log("Unrecognized item in database.");
					break;
			}

			database.Add(itemIn);
		}
	}
}