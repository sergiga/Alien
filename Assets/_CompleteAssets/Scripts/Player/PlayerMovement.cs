using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	public float speed = 5f;			// Player speed.
	public float dragDistance = 0.3f;	// The distance the player drags after change direction.
	public float explosionForce = 650f;
	public Transform sparklePrefab;

	Animator playerAnim;
	Rigidbody2D rb;
	Vector3 startPosition;
	Vector3 playerViewportPosition;
	Vector3 movement;					// The vector with the direction of the player's movement.
	float orientation = 0.8f;			// Orientation of the player's movement.
	float dragTime;						// Initial drag time.
	float dragJourney;					// Percentaje complete of the drag.
	float minPosition;					// Initial position before dragging.
	float maxPosition;					// Final position after dragging.
	bool canChangeDirection = false;
	bool isDeath = false;

	void Awake() {
		startPosition = transform.position;
		playerAnim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
	}

	void OnEnable() {
		playerAnim.SetTrigger ("move");
	}

	void Update() {
		if(Input.GetMouseButtonDown(0) && !canChangeDirection && !isDeath) {
			Drag ();
		}
		if (transform.position.y > 6 && isDeath) {
			Instantiate(sparklePrefab, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
		playerViewportPosition = Camera.main.WorldToViewportPoint (transform.position);
		if (playerViewportPosition.x < 0.0f || playerViewportPosition.x > 1.0f) {
			canChangeDirection = false;
		}
		if (playerViewportPosition.x < -0.05f) {
			orientation = -0.8f;
			transform.position = new Vector3(3.4f, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);
		} 
		else if (playerViewportPosition.x > 1.05f) {
			orientation = 0.8f;
			transform.position = new Vector3(-3.4f, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		}
	}

	void FixedUpdate () {
		if (!isDeath) {
			Move ();
		}
	}

	void Drag() {
		minPosition = transform.position.x;
		maxPosition = minPosition + (dragDistance * orientation);
		orientation *= (-1);
		transform.localScale = new Vector3(orientation, 0.8f, 0.8f);
		dragTime = Time.time;
		canChangeDirection = true;
	}

	void Move() {
		movement.Set (orientation, 0f, 0f);
		movement = movement * speed * Time.deltaTime;
		if (!canChangeDirection)
			transform.position += movement;
		else {
			dragJourney = (Time.time - dragTime) * 6f;
			transform.position = new Vector3(Mathf.Lerp (minPosition, maxPosition, dragJourney)
			                                 , transform.position.y
			                                 , transform.position.z);
			if(dragJourney >= 1)
				canChangeDirection = false;
		}
	}

	public void playerDeath() {
		isDeath = true;
		playerAnim.SetTrigger ("Death");
		rb.AddForce (transform.up * explosionForce);
	}
}
