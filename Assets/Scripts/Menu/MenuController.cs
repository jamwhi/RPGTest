using UnityEngine;
using System.Collections.Generic;
using System;

public class MenuController : MonoBehaviour {

    public GameObject menu;
    public GameObject inventory;
    public GameObject shop;
    public GameObject character;
    public GameObject settings;
    public GameObject crafting;
    public GameObject windowContainer;

    public MouseController mouseController;
    public AudioController audioController;
    public AudioClip menuSound;

    private int frontWindow;

    void Awake() {
        frontWindow = windowContainer.transform.childCount - 1;
    }


	void Update () {

		// Open menu
		if (Input.GetKeyDown(KeyCode.Escape)) {
            EscapePressed();
		}

		// Open inventory
		if (Input.GetKeyDown(KeyCode.I)) {
            ToggleWindow(inventory);
		}

		// Open character panel (equipment)
		if (Input.GetKeyDown(KeyCode.C)) {
            ToggleWindow(character);
		}
	}


    public void EscapePressed() {
        GameObject frontObject = windowContainer.transform.GetChild(frontWindow).gameObject;
        if (frontObject.activeSelf) {
            if (frontObject.name == "InventoryPanel") {
                shop.transform.SetAsFirstSibling();
                shop.SetActive(false);
            }
            ToggleWindow(frontObject);
        }
        else {
            ToggleWindow(menu);
        }
    }

    public void CheckForWindows() {
        GameObject frontObject = windowContainer.transform.GetChild(frontWindow).gameObject;
        if (!frontObject.activeSelf) {
            mouseController.DestroyBackBlocker();
        }
    }

    public void ToggleWindow(GameObject toggled) {
        toggled.SetActive(!toggled.activeSelf);
        if (toggled.activeSelf) {
            toggled.transform.SetAsLastSibling();
            if(mouseController.backBlock == null) {
                mouseController.CreateBackBlocker();
            }
        }
        else {
            toggled.transform.SetAsFirstSibling();
            CheckForWindows();
        }
		audioController.PlaySfx(menuSound);
	}

	public void OpenShop() {
        DeselectInventory();
        shop.SetActive(true);
		inventory.SetActive(true);
        inventory.transform.SetAsLastSibling();
        shop.transform.SetAsLastSibling();
        audioController.PlaySfx(menuSound);
        if(mouseController.backBlock == null) {
            mouseController.CreateBackBlocker();
        }
	}

	public void CloseShop () {
        if (inventory.activeSelf) {
            DeselectInventory();
            shop.SetActive(false);
            inventory.SetActive(false);
            shop.transform.SetAsLastSibling();
            inventory.transform.SetAsLastSibling();
            CheckForWindows();
            audioController.PlaySfx(menuSound);
        }
	}

    public void ShopButton() {
        ToggleWindow(shop);
        DeselectInventory();
    }

    public void DeselectInventory() {
        inventory.GetComponent<Inventory>().DeselectAll();
        shop.GetComponent<Inventory>().DeselectAll();
    }

    public void CharButton() {
        ToggleWindow(character);
    }

    public void CraftButton() {
        ToggleWindow(crafting);
    }

}
