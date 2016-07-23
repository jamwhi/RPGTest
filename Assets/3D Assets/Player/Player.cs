using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public GameObject fallParticle;

	private bool alive = true;
	private Rigidbody rb;
	private Vector3 startPos;

	void Start() {
		rb = GetComponent<Rigidbody>();
		startPos = transform.position;
	}

	void FixedUpdate () {
		if (transform.position.y < -5f) {
			return;
		}

		Vector3 force = new Vector3(Input.GetAxis("Horizontal") * speed, 0f, Input.GetAxis("Vertical") * speed);
		rb.AddForce(force, ForceMode.Force);
	}

	void OnTriggerEnter() {
		// placeholder
	}

	void OnTriggerExit () {
		// placeholder
	}

	void OnCollisionEnter (Collision col) {
		if (!alive) {
			return;
		}
		if (col.collider.gameObject.name == "LowerGround") {
			StartCoroutine(Fall());
		}
	}

	IEnumerator Fall() {
		alive = false;
		GameObject fall = Instantiate(fallParticle, transform.position, Quaternion.identity) as GameObject;
		fall.transform.parent = transform.parent;
		this.enabled = false;
		yield return new WaitForSeconds(3);
		transform.position = startPos;
		this.enabled = true;
		alive = true;
	}
}
