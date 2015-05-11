using UnityEngine;
using System.Collections;

public class SparkleMovement : MonoBehaviour {

	public float fallSpeed = 3f;
	public float rotSpeed = 3f;

	private Vector3 initialPosition;
	private Vector3 finalPosition;
	private float startTime;
	private float totalRotation = 0f;
	private float totalDistance;

	void Start() {
		startTime = Time.time;
		initialPosition = new Vector3 (transform.position.x, 6f, 0.453f);
		finalPosition = new Vector3 (transform.position.x, 1f, 0.453f);
		totalDistance = Vector3.Distance (initialPosition, finalPosition);
		transform.position = initialPosition;
	}

	void Update() {
		float distCovered = (Time.time - startTime) * fallSpeed;
		float fracJourney = distCovered / totalDistance;
		totalRotation = totalRotation + Time.deltaTime * rotSpeed;
		transform.position = Vector3.Lerp (initialPosition, finalPosition, fracJourney);
		transform.localScale = new Vector3 (1f-fracJourney, 1f-fracJourney, 1f-fracJourney);
		transform.Rotate(0f, 0f, totalRotation); 

		if (transform.position.y < 1.5) {
			Destroy(gameObject);
		}
	}
}
