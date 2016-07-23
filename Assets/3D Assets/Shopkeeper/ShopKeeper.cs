using UnityEngine;
using UnityEngine.EventSystems;

public class ShopKeeper : MonoBehaviour, IPointerClickHandler {

	private bool playerNear = false;

	public void OnPointerClick (PointerEventData eventData) {
		Debug.Log("Clicked");
		if (playerNear) {
				AssetManager.GetInstance().menuController.OpenShop();
		}
	}

	void OnTriggerEnter (Collider other) {
		Debug.Log("Enter");
		if (other.tag == "Player") {
			playerNear = true;
		}
	}

	void OnTriggerExit (Collider other) {
		Debug.Log("Exit");
		if (other.tag == "Player") {
			playerNear = false;
		}
	}
}
