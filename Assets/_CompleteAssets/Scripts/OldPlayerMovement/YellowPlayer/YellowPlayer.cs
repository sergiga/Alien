using UnityEngine;
using System.Collections;

[RequireComponent (typeof (YellowPlayerController))]
public class YellowPlayer : PlayerMove {
	
	public float jumpHeight = 4;
	public float timeToJumpApex = .4f;
	
	float gravity;
	float jumpVelocity;
	float direction;

	float accelerationTimeAir = .2f;
	float accelerationTimeGrounded = .1f;
	float jumpDistanceX = 4f;
	
	Vector3 velocity;
	
	float velocityXSmoothing;

	float changeDirection = 1;

	bool moveTrigger = false;
	bool landing = false;

	YellowPlayerController controller;
	Animator animator;

	protected override void Start () {

		animator = GetComponent<Animator> ();
		controller = GetComponent<YellowPlayerController> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		base.Start ();
	}
	
	void Update () {

		if (Input.GetMouseButtonDown(0)) {
			changeDirection *= -1;
		}

		if (!GameManager.instance.move || landing) return;
		if (!moveTrigger)
			StartMove ();

		CheckLimits ();

		if (controller.collisions.below && velocity.y < 0) {
			landing = true;
			StartCoroutine (Landing());
			velocity.x = changeDirection;
			direction = changeDirection;
		}

		float targetVelocityX = direction * jumpDistanceX;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAir);
		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}

	void StartMove() {

		moveTrigger = true;
		animator.SetTrigger ("jump");
	}

	IEnumerator Landing() {

		animator.SetTrigger ("land");
		yield return new WaitForSeconds(0.2f);
		velocity.y = jumpVelocity;
		landing = false;
	}
}
