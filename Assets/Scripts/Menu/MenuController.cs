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


	void Update () {

		// Open menu
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ToggleMenu(menu);
		}

		// Open inventory
		if (Input.GetKeyDown(KeyCode.I)) {
			ToggleMenu(inventory);
		}

		// Open character panel (equipment)
		if (Input.GetKeyDown(KeyCode.C)) {
			ToggleMenu(character);
		}
	}


	public void ToggleMenu (GameObject toggled) {
		toggled.SetActive(!toggled.activeSelf);
		audioController.PlaySfx(menuSound);
	}

	public void OpenShop() {
		shop.SetActive(true);
		inventory.SetActive(true);
		audioController.PlaySfx(menuSound);
	}

	public void CloseShop () {
		shop.SetActive(false);
		inventory.SetActive(false);
		audioController.PlaySfx(menuSound);
	}

	public void MenutoInventory() {
        DoButtonPress(menu, inventory);
    }

    public void InventoryToMenu() {
		//shop.SetActive(false);
		//character.SetActive(false);
		//crafting.SetActive(false);
		//DoButtonPress(inventory, menu);
		inventory.SetActive(false);
		audioController.PlaySfx(menuSound);
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
