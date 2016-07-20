using UnityEngine;
using System.Collections;
using System;

public class MenuController : MonoBehaviour {

    public GameObject menu;

    public GameObject inventory;
    public GameObject shop;
    public GameObject character;
    public GameObject settings;
    public GameObject crafting;

    public AudioController audioController;
    public AudioClip menuSound;

    public void MenutoInventory() {
        DoButtonPress(menu, inventory);
    }

    public void InventoryToMenu() {
        shop.SetActive(false);
        character.SetActive(false);
        crafting.SetActive(false);
        DoButtonPress(inventory, menu);
    }

    public void SettingsToMenu() {
        DoButtonPress(settings, menu);
    }

    public void MenuToSettings() {
        DoButtonPress(menu, settings);
    }

    public void CraftingToMenu() {
        DoButtonPress(crafting, menu);
    }

    public void ShopButton() {
        shop.SetActive(!shop.activeSelf);
        audioController.PlaySfx(menuSound);
        inventory.GetComponent<Inventory>().DeselectAll();
        shop.GetComponent<Inventory>().DeselectAll();
    }

    public void CharButton() {
        character.SetActive(!character.activeSelf);
        audioController.PlaySfx(menuSound);
    }

    public void CraftButton() {
        crafting.SetActive(!crafting.activeSelf);
        audioController.PlaySfx(menuSound);
    }

    public void DoButtonPress(GameObject closed, GameObject opened) {
        opened.SetActive(true);
        closed.SetActive(false);
        audioController.PlaySfx(menuSound);
    }

}
