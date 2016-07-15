using UnityEngine;
using System.Collections;

public class Transaction : MonoBehaviour {

    public AudioSource audioController;
    public AudioClip sellItem;

	// Use this for initialization
	void Start () {	
	}
	
	// Update is called once per frame
	void Update () {	
	}
    private void updateGold(Inventory toAdd, Inventory toSub, int amount) {
        toAdd.goldAmount += amount;
        toAdd.goldDisplay.text = toAdd.goldAmount.ToString();
        toSub.goldAmount -= amount;
        toSub.goldDisplay.text = toSub.goldAmount.ToString();

    }
    public bool ItemTransaction(ItemData itemIn, Inventory invIn, float valueMod = 1.0f) {
        // IF same owner, do nothing
        if(itemIn.owner == invIn) {
            Debug.Log("Same item owner as inventory in Transaction");
            return true;
        }
        // ELSE IF itemIn from character(0), into shop(1)
        else if( (itemIn.owner.invType == 0) && (invIn.invType == 1) ){
            this.updateGold(itemIn.owner, invIn, itemIn.item.Value);
            Destroy(itemIn.gameObject);
            audioController.PlayOneShot(sellItem);
            Debug.Log("Item sold to shop");
            return false;
        }
        // ELSE IF itemIn from shop(1), into character(0)
        else if( (itemIn.owner.invType == 1) && (invIn.invType == 0)) {
            this.updateGold(itemIn.owner, invIn, itemIn.item.Value);
            //audioController.PlayOneShot(sellItem);
            Debug.Log("Item bought by character");
            return true;

        }
        return true;    
    }
}
