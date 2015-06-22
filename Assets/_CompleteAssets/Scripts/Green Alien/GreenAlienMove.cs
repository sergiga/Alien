using UnityEngine;
using System.Collections;

public class GreenAlienMove : MonoBehaviour {
	
	public float moveSpeed = 6f;
	public float dragTime;
	public float accelerationDragging = .2f;
	public AudioClip turn;

	float explosionForceY = 100f;
	float gravity = -50f;
	float direction = 1f;
	
	float velocityXSmoothing;
	float accelerationTime = .1f;
	float accelerationTimeDragging;
	float dragLeft;

	Vector3 rotation;
	Vector3 velocity;
	Vector3 screenBorder;

	bool death = true;
	bool dragging = false;
	bool moveTrigger = false;

	Animator animator;
	PlayerController controller;

	void Start () {
		
		animator = GetComponent<Animator> ();
		controller = GetComponent<PlayerController> ();

		screenBorder = Camera.main.ViewportToWorldPoint (screenBorder);
		screenBorder = new Vector3 (screenBorder.x, -1.671f, 0f);
		rotation = new Vector3 (0f, 0f, 0f);

	}
	
	void Update () {

		if (GameManager.instance.gameOver && !death) {
			death = true;
			StartCoroutine(controller.Death());
		}

		if (!GameManager.instance.move) return;
		if (!moveTrigger)
			StartMove ();

		if (Input.GetMouseButtonDown(0) && !dragging) {
			RotateY();
		}

		if (dragging) {
			dragLeft -= Time.deltaTime;
			accelerationTimeDragging += Time.deltaTime;
			if (dragLeft < 0) {
				accelerationTimeDragging =  accelerationDragging;
				dragging = false;
				direction *= -1;
			}
		}

		if (controller.collisions.below) {
			velocity.y = 0;
		}

		CheckLimits ();
		
		float targetVelocityX = direction * moveSpeed;

		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (dragging) ? accelerationTimeDragging : accelerationTime);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	private void CheckLimits() {
		
		Vector3 playerPosition = Camera.main.WorldToViewportPoint (transform.position);
		Vector3 border = screenBorder;
		
		if (playerPosition.x > 1.02f) {
			border = screenBorder;
			transform.position = border;
		} 
		else if (playerPosition.x < -0.02f) {
			screenBorder.x *= -1;
			transform.position = border;
		}
	}

	private void RotateY() {

		if (rotation.y == 0)
			rotation.y = 180;
		else 
			rotation.y = 0;
		dragging = true;
		velocity.x = 0;
		direction *= -1;
		dragLeft = dragTime;
		accelerationTimeDragging = accelerationDragging;
		transform.eulerAngles = rotation;
		SoundManager.instance.RandomizeSfx(turn);
	}

	private void StartMove() {
		moveTrigger = true;
		death = false;
		animator.SetTrigger ("move");
	}
}
