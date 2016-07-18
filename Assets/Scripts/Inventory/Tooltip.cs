using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Tooltip : MonoBehaviour {

    private Item item;    
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
    public void Activate(Item item, Vector2 pos){

        this.item = item;
        tooltip.SetActive(true);
        ConstructDataString();       
    }
    
    // Disable the tooltip
    public void Deactivate(){

        tooltip.SetActive(false);
    }

    private void ConstructStatString() {

        statText.text = "";
        string powerType = "";

        switch (item.ItemType) {
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

        switch (item.ItemType) {

            case "Weapon":
            case "Armor":
            case "Shield":
                powerText.text = item.Power.ToString() + powerType;
                duraText.text = item.Durability.ToString() + " Durability";
                if(item.Strength > 0) statText.text += "+" + item.Strength.ToString() + " Strength\n";
                if(item.Dexterity > 0) statText.text += "+" + item.Dexterity.ToString() + " Dexterity\n";
                if(item.Magic > 0) statText.text += "+" + item.Magic.ToString() + " Magic\n";
                if(item.Vitality > 0) statText.text += "+" + item.Vitality.ToString() + " Vitality\n";
                statText.text = statText.text.Remove(statText.text.Length - 1);
                break;

            case "Consumable":
                if(item.HealthRestore > 0) statText.text += item.HealthRestore.ToString() + " Health Restoration\n";
                if (item.ManaRestore > 0) statText.text += item.ManaRestore.ToString() + " Mana Restoration\n";
                statText.text += item.Uses.ToString() + "Uses";
                powerText.gameObject.SetActive(false);
                duraText.gameObject.SetActive(false);
                break;

            case "Ammo":
                statText.gameObject.SetActive(false);
                duraText.gameObject.SetActive(false);
                break;

            case "Tool":
                statText.text += item.Hardness.ToString() + " Hardness";
                duraText.text = item.Durability.ToString() + " Durability";
                powerText.gameObject.SetActive(false);
                break;

            case "Material":
                statText.text += item.Quality.ToString() + " Quality";
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
        titleText.text = "<b>" + item.Title + "</b>\n";
        titleText.color = rarityColors[item.Rarity - 1];
        if (item.ItemType == "Material") {
            titleText.text += "<color=#595959FF>" + item.MatType + "</color>";
        } else {
            titleText.text += "<color=#595959FF>" + item.ItemType + "</color>";
        }

        // Set stat text
        powerText.gameObject.SetActive(true);
        statText.gameObject.SetActive(true);
        duraText.gameObject.SetActive(true);
        ConstructStatString();

        // Set Description text 
        descText.text = "<color=#000000>" + item.Description + "</color>";

        // Set Value text
        valText.text = item.Value.ToString() + " Gold";
    }

    
}
