using UnityEngine;

public class Player : MonoBehaviour {

	public float speed;
	public float deathY;

	private Rigidbody rb;
	private Vector3 startPos;
	public MenuController menuController;

	void Start() {
		rb = GetComponent<Rigidbody>();
		menuController = GameObject.FindWithTag("UI").GetComponent<MenuController>();
		startPos = transform.position;
	}

	void FixedUpdate () {
		Vector3 force = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
		rb.AddForce(force, ForceMode.Force);

		if (transform.position.y < deathY) {
			transform.position = startPos;
		}
	}

	void OnTriggerEnter() {
		menuController.OpenShop();
	}

	void OnTriggerExit () {
		menuController.CloseShop();
	}
}
