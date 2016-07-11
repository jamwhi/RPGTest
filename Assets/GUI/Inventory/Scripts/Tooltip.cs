using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tooltip : MonoBehaviour {

    private Item item;
    private GameObject tooltip;

    public Text titleText;
    public Text statText;
    public Text descText;
    public Text valText;
 
    // Activate the tooltip with an item

    void Start(){

        tooltip = GameObject.FindWithTag("InventoryTooltip");
        tooltip.SetActive(false);
    }

    void Update(){

        Vector2 mousePos = (Vector2)(Input.mousePosition);
        tooltip.transform.position = mousePos;
    }

    public void Activate(Item item, Vector2 pos){

        this.item = item;
        tooltip.SetActive(true);
        ConstructDataString();       
    }
    
    public void Deactivate(){

        tooltip.SetActive(false);
    }

    void ConstructDataString(){

        titleText.text = "<color=#C200CE><b>" + item.Title + "</b></color>";
        statText.text = "<color=#970000>Power:" + item.Power.ToString() + "</color>\n";
        statText.text += "<color=#003397>Durability: " + item.Durability.ToString() + "</color>";
        descText.text = "<color=#000000>" + item.Description + "</color>";
        valText.text = item.Value.ToString() + " Gold";
    }

    
}
