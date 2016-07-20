using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour {

    private Item item;
    private Recipe recipe;
    private Color[] rarityColors = new Color[4] { new Color(0.3f, 0.3f, 0.3f), new Color(0f, 0.65f, 0f), new Color(0f, 0f, 1f), new Color(0.8f, 0f, 0.8f) };

    // Color hex values: { "#6A6A6A", "#02BD0B", "#0000FF", "#C200CE" };
    public GameObject tooltip;
    public Text titleText;
    public Text powerText;
    public Text statText;
    public Text descText;
    public Text duraText;
    public Text valText;

    void Start(){
        tooltip.SetActive(false);
    }

    void Update(){
        // Place item icon at mouse location
        Vector2 mousePos = (Vector2)(Input.mousePosition);
        tooltip.transform.position = mousePos;
    }

    // Enable the tooltip
    public void ActivateItem(Item item, Vector2 pos){

        this.item = item;
        tooltip.SetActive(true);
        ConstructDataString();       
    }

    public void ActivateRecipe(Recipe recipe, Vector2 pos) {
        this.recipe = recipe;
        tooltip.SetActive(true);
        ConstructRecipeString();
    }
    
    // Disable the tooltip
    public void Deactivate(){

        this.item = null;
        this.recipe = null;
        tooltip.SetActive(false);
    }

    private void ConstructStatString() {

        statText.text = "";
        string powerType = "";

        switch (item.itemType) {
            case "Weapon":
                powerType = " Damage";
                break;
            case "Armor":
                powerType = " Armor";
                break;
            case "Shield":
                powerType = " Block";
                break;
        }

        switch (item.itemType) {

            case "Weapon":
            case "Armor":
            case "Shield":
                powerText.text = item.power.ToString() + powerType;
                duraText.text = item.durability.ToString() + " Durability";
                if(item.strength > 0) statText.text += "+" + item.strength.ToString() + " Strength\n";
                if(item.dexterity > 0) statText.text += "+" + item.dexterity.ToString() + " Dexterity\n";
                if(item.magic > 0) statText.text += "+" + item.magic.ToString() + " Magic\n";
                if(item.vitality > 0) statText.text += "+" + item.vitality.ToString() + " Vitality\n";
                statText.text = statText.text.Remove(statText.text.Length - 1);
                break;

            case "Consumable":
                if(item.healthRestore > 0) statText.text += item.healthRestore.ToString() + " Health Restoration\n";
                if (item.manaRestore > 0) statText.text += item.manaRestore.ToString() + " Mana Restoration\n";
                statText.text += item.uses.ToString() + "Uses";
                powerText.gameObject.SetActive(false);
                duraText.gameObject.SetActive(false);
                break;

            case "Ammo":
                statText.gameObject.SetActive(false);
                duraText.gameObject.SetActive(false);
                break;

            case "Tool":
                statText.text += item.hardness.ToString() + " Hardness";
                duraText.text = item.durability.ToString() + " Durability";
                powerText.gameObject.SetActive(false);
                break;

            case "Material":
                statText.text += item.quality.ToString() + " Quality";
                powerText.gameObject.SetActive(false);
                duraText.gameObject.SetActive(false);
                break;

            default:
                Debug.Log("Unrecognized item in database.");
                break;
        }
    }

    void ConstructDataString(){

        // Set title text
        titleText.text = "<b>" + item.title + "</b>\n";
        titleText.color = rarityColors[item.rarity - 1];
        if (item.itemType == "Material") {
            titleText.text += "<color=#595959FF>" + item.matType + "</color>";
        } else {
            titleText.text += "<color=#595959FF>" + item.itemType + "</color>";
        }

        // Set stat text
        powerText.gameObject.SetActive(true);
        statText.gameObject.SetActive(true);
        duraText.gameObject.SetActive(true);
        ConstructStatString();

        // Set Description text 
        descText.text = "<color=#000000>" + item.description + "</color>";

        // Set Value text
        valText.gameObject.SetActive(true);
        valText.text = item.value.ToString() + " Gold";
    }

    void ConstructRecipeString() {
        titleText.text = "<b>" + recipe.title + "</b>\n<color=#595959FF>" + recipe.itemType + "</color>";
        powerText.text = "Tool required: " + recipe.tool;

        statText.text = "";
        if (recipe.metal > 0) statText.text += recipe.metal.ToString() + " Metal\n";
        if (recipe.plating > 0) statText.text += recipe.plating.ToString() + " Plating\n";
        if (recipe.blade > 0) statText.text += recipe.blade.ToString() + " Blade\n";
        if (recipe.wood > 0) statText.text += recipe.wood.ToString() + " Wood\n";
        if (recipe.leather > 0) statText.text +=  recipe.leather.ToString() + " Leather\n";    
        if (recipe.hilt > 0) statText.text += recipe.hilt.ToString() + " Hilt\n";
        statText.text = statText.text.Remove(statText.text.Length - 1);
        descText.text = recipe.description;

        valText.gameObject.SetActive(false);
        duraText.gameObject.SetActive(false);
    }
    
}
