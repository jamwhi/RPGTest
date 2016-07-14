using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    public GameObject inventory;
    public GameObject menu;
    public GameObject shop;

    public AudioSource guiAudio;
    public AudioClip menuSound;

    public void MenutoInventory() {
        DoButtonPress(menu, inventory);
    }

    public void InventoryToMenu() {
        DoButtonPress(inventory, menu);
    }

    public void DoButtonPress(GameObject closed, GameObject opened) {
        opened.SetActive(true);
        closed.SetActive(false);
        guiAudio.PlayOneShot(menuSound);
    }
}
