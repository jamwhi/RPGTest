using UnityEngine;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;

public class RecipeDatabase : MonoBehaviour {

    // Database of Recipes
    private List<Recipe> database = new List<Recipe>();
    private JsonData recipesInDatabase;

    void Start() {

        recipesInDatabase = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Recipes.json"));
        ConstructRecipeDatabase();
    }
    
    public Recipe FetchRecipeByID(int id) {

        for (int i = 0; i < database.Count; i++) {
            if (database[i].ID == id) {
                return database[i];
            }
        }
        return new Recipe();
    }
    

    private void ConstructRecipeDatabase() {

        // Loop over each item in recipesDatabase
        for (int i = 0; i < recipesInDatabase.Count; i++) {
            // Loop over each component
            Dictionary<string, int> components = new Dictionary<string, int>();                  
            ICollection<string> keys = recipesInDatabase[i]["components"].Keys;
                foreach (var key in keys) {
                    components.Add(key.ToString(), (int)recipesInDatabase[i]["components"][key]);
                }
            Recipe newRecipe = new Recipe(
            (int)recipesInDatabase[i]["id"],
            recipesInDatabase[i]["title"].ToString(),
            recipesInDatabase[i]["itemType"].ToString(),
            components,
            recipesInDatabase[i]["slug"].ToString()
            );
            database.Add(newRecipe);   
        }
    }
}
