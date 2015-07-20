using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	public float cloudVelocity = 1f;
	public float lifeTime = 25f;

	Vector3 velocity;

	int orderInLayer;

	float cloudProgress;
	float velocityXSmoothing;

	void Start () {

		cloudVelocity = cloudVelocity;
		cloudVelocity *= (transform.position.x < 0) ? 1 : -1;
		cloudProgress = lifeTime;
		orderInLayer = Random.Range (0, 3);
		GetComponent<SpriteRenderer> ().sortingOrder = orderInLayer;
	}
	
	void Update () {

		cloudProgress -= Time.deltaTime;
		if (cloudProgress < 0) {
			Destroy(gameObject);
		}
		velocity.x = Mathf.SmoothDamp (velocity.x, cloudVelocity, ref velocityXSmoothing, 10f);
		transform.Translate (velocity * Time.deltaTime);
	}
}
