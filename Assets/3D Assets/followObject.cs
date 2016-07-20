using UnityEngine;

public class followObject : MonoBehaviour {

	public Transform target;
	public bool x, y, z;

	public float smooth;

	private Vector3 offset;

	void Start() {
		if (!target) {
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}

		offset = target.position - transform.position;
	}

	void LateUpdate () {
		Vector3 newPos = new Vector3(
			x ? target.position.x - offset.x : transform.position.x,
			y ? target.position.y - offset.y : transform.position.y,
			z ? target.position.z - offset.z : transform.position.z
		);

		if (smooth == 0) {
			transform.position = newPos;
		} else {
			transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
			//transform.position = Vector3.Lerp(transform.position, newPos, smooth / 8);
			//transform.position = Vector3.SmoothDamp(transform.position, newPos, , smooth);

			//transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, newRot, smooth * Time.deltaTime);
		}

		//transform.position = newPos;
	}
}