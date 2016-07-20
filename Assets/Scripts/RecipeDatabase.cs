using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;

public class RecipeDatabase : MonoBehaviour {

    // Database of items
    public List<Recipe> database = new List<Recipe>();
    private string JString;

    void Awake() {

        JString = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Recipes.json")).ToJson();
        Recipe[] recipes = JsonMapper.ToObject<Recipe[]>(JString);

        for (int i = 0; i < recipes.Length; i++) {
            recipes[i].SetSprite();
            database.Add(recipes[i]);
        }
    }

    public Recipe FetchRecipeByID(int id) {

        for (int i = 0; i < database.Count; i++) {
            if (database[i].id == id) {
                return database[i];
            }
        }
        return new Recipe();
    }
}
