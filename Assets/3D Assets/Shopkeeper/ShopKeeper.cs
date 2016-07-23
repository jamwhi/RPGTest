using UnityEngine;
using UnityEngine.EventSystems;

public class ShopKeeper : MonoBehaviour, IPointerClickHandler {

	private bool playerNear = false;

	public void OnPointerClick (PointerEventData eventData) {
		if (playerNear) {
				AssetManager.GetInstance().menuController.OpenShop();
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			playerNear = true;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			playerNear = false;
		}
	}
}
