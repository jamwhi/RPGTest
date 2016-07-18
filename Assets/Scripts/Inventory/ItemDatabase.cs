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
            Item itemIn = new Item(
                (int)itemsInDatabase[i]["id"],
                itemsInDatabase[i]["title"].ToString(),
                (int)itemsInDatabase[i]["val"],
                (int)itemsInDatabase[i]["charSlot"],
                itemsInDatabase[i]["itemType"].ToString(),
                itemsInDatabase[i]["description"].ToString(),
                (bool)itemsInDatabase[i]["stackable"],
                (int)itemsInDatabase[i]["maxStack"],
                (bool)itemsInDatabase[i]["useable"],
				(int)itemsInDatabase[i]["rarity"],
                itemsInDatabase[i]["slug"].ToString()
				);

            switch (itemIn.ItemType) {

                case "Weapon":
                case "Armor":
                case "Shield":
                    itemIn.Power = (int)itemsInDatabase[i]["power"];
                    itemIn.Strength = (int)itemsInDatabase[i]["strength"];
                    itemIn.Dexterity = (int)itemsInDatabase[i]["dexterity"];
                    itemIn.Magic = (int)itemsInDatabase[i]["magic"];
                    itemIn.Vitality = (int)itemsInDatabase[i]["vitality"];
                    itemIn.Durability = (int)itemsInDatabase[i]["durability"];
                    break;

                case "Consumable":
                    itemIn.Uses = (int)itemsInDatabase[i]["uses"];
                    itemIn.HealthRestore = (int)itemsInDatabase[i]["healthRestore"];
                    itemIn.ManaRestore = (int)itemsInDatabase[i]["manaRestore"];
                    break;

                case "Ammo":
                    itemIn.Power = (int)itemsInDatabase[i]["power"];
                    break;

                case "Tool":
                    itemIn.Hardness = (int)itemsInDatabase[i]["hardness"];
                    itemIn.Durability = (int)itemsInDatabase[i]["durability"];
                    break;

                case "Material":
                    itemIn.MatType = itemsInDatabase[i]["matType"].ToString();
                    itemIn.Quality = (int)itemsInDatabase[i]["quality"];
                    break;

                default:
                    Debug.Log("Unrecognized item in database.");
                    break;
            }

            database.Add(itemIn);
        }
	}
}