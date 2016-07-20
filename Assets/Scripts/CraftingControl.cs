using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingControl : MonoBehaviour {

    public Inventory components;
    public Text componentText;
    public Recipe selectedRecipe;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RecipeSelected(Recipe recipe) {
        selectedRecipe = recipe;
        SetComponentText();
    }

    void SetComponentText() {
        componentText.text = "Requires:\n";
        Debug.Log(selectedRecipe.hilt);
        Debug.Log(selectedRecipe.diamond);
        if (selectedRecipe.metal > 0) componentText.text += selectedRecipe.metal.ToString() + " Metal\n";
        if (selectedRecipe.plating > 0) componentText.text += selectedRecipe.plating.ToString() + " Plating\n";
        if (selectedRecipe.blade > 0) componentText.text += selectedRecipe.blade.ToString() + " Blade\n";
        if (selectedRecipe.wood > 0) componentText.text += selectedRecipe.wood.ToString() + " Wood\n";
        if (selectedRecipe.leather > 0) componentText.text += selectedRecipe.leather.ToString() + " Leather\n";
        if (selectedRecipe.hilt > 0) componentText.text += selectedRecipe.hilt.ToString() + " Hilt\n";
        if (selectedRecipe.diamond > 0) componentText.text += selectedRecipe.diamond.ToString() + " Diamond\n";
        componentText.text = componentText.text.Remove(componentText.text.Length - 1);
    }
}
