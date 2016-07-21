using UnityEngine;

public class KillAfterTime : MonoBehaviour {

	public float time;

	void Update () {
		time -= Time.deltaTime;
		if (time <= 0f) {
			Destroy(gameObject);
		}
	}
}
