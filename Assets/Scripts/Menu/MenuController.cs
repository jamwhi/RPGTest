using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public GameObject menu;

    public GameObject inventory;
    public GameObject shop;

    public AudioSource guiAudio;
    public AudioClip menuSound;

    public void MenutoInventory() {
        DoButtonPress(menu, inventory);
    }

    public void InventoryToMenu() {
        DoButtonPress(inventory, menu);
        shop.SetActive(false);
    }

    public void ShopButton() {
        shop.SetActive(!shop.activeSelf);
        guiAudio.PlayOneShot(menuSound);
        inventory.GetComponent<Inventory>().DeselectAll();
        shop.GetComponent<Inventory>().DeselectAll();
    }

    public void DoButtonPress(GameObject closed, GameObject opened) {
        opened.SetActive(true);
        closed.SetActive(false);
        guiAudio.PlayOneShot(menuSound);

    }
}
