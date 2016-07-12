using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour {

    private Item item;
    private GameObject tooltip;   
    private Color[] rarityColors = new Color[4] { new Color(0.3f, 0.3f, 0.3f), new Color(0f, 0.65f, 0f), new Color(0f, 0f, 1f), new Color(0.8f, 0f, 0.8f) };
    // Color hex values: { "#6A6A6A", "#02BD0B", "#0000FF", "#C200CE" };

    public Text titleText;
    public Text statText;
    public Text descText;
    public Text valText;

    void Start(){
        // Tooltip begins disabled
        tooltip = GameObject.FindWithTag("InventoryTooltip");
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

    void ConstructDataString(){

        // Set title text
        titleText.text = "<b>" + item.Title + "</b>";
        titleText.color = rarityColors[item.Rarity - 1];

        // Set Stat text
        if (item.ItemType == "Weapon") {
            statText.text = "<color=#970000>Attack: " + item.Power.ToString() + "</color>\n";
            statText.text += "<color=#003397>Durability: " + item.Durability.ToString() + "</color>";
        }
        else if (item.ItemType == "Armor") {
            statText.text = "<color=#2F6502>Defense: " + item.Power.ToString() + "</color>\n";
            statText.text += "<color=#003397>Durability: " + item.Durability.ToString() + "</color>";
        }
        else if (item.ItemType == "Useable") {
            statText.text = "<color=#2F6502>Strength: " + item.Power.ToString() + "</color>\n";
            statText.text += "<color=#003397>Uses: " + item.Durability.ToString() + "</color>";
        }

        // Set Description text 
        descText.text = "<color=#000000>" + item.Description + "</color>";

        // Set Value text
        valText.text = item.Value.ToString() + " Gold";
    }

    
}
