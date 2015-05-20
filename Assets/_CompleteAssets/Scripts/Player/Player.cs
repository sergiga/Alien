using UnityEngine;
using System.Collections;

public class Player : PlayerMove {

	public AudioClip turn;
	public float speed = 6f;
	public float dragDistance = 0.3f;

	private IEnumerator drag;
	private Animator animator;
	private Vector3 direction;
	private Vector3 scale;
	private float blink = 2f;
	private float breath = 2f;
	private bool controllable = true;
	private bool changing = false;
	private bool moveTrigger = false;
	private bool death = true;

	protected override void Start () {
		animator = GetComponent<Animator> ();
		direction = new Vector3 (0.8f, 0f, 0f);
		scale = new Vector3 (0.8f, 0.8f, 0.8f);
		drag = Drag (transform.position);
		base.Start ();
	}

	private void Update() {

		CheckBlink ();
		if (GameManager.instance.gameOver && !death) {
			name = "DeathPlayer";
			death = true;
			StopCoroutine ( drag);
			StartCoroutine( Death());
		}
		if (GameManager.instance.changeCharacter && !changing) {
			ChangeCharacter ();
		}
		if (!GameManager.instance.move || !controllable) return;

		if (!moveTrigger) {
			StartMove ();
		}
		if (Input.GetMouseButtonDown (0) && !death) {
			Turn();
		}

		CheckLimits ();
		CheckBreath ();
		Move (direction, speed * Time.deltaTime);
	}

	private void Turn() {

		controllable = false;
		SoundManager.instance.RandomizeSfx(turn);
		scale = new Vector3(scale.x * (-1), 0.8f, 0.8f);
		ChangeSide (scale);
		drag = Drag (transform.position);
		StartCoroutine(drag);
	}

	private void CheckBlink() {

		blink -= Time.deltaTime;
		if (blink < 0) {
			blink = Random.Range(4.0f, 5.0f);
			animator.SetTrigger("blink");
		}
	}

	private void CheckBreath() {
	
		breath -= Time.deltaTime;
		if (breath < 0) {
			breath = Random.Range(4.0f, 5.0f);
			animator.SetTrigger("breath");
		}
	}

	private IEnumerator Drag (Vector3 start) {

		Vector3 target = new Vector3(dragDistance, 0f, 0f);
		float time = Time.time;
		float dragJourney = 0f;
	
		target += transform.position;
		while (dragJourney <= 1) {
			dragJourney = (Time.time - time) * 4f;
			transform.position = Vector3.Lerp (start, target, dragJourney);
			yield return new WaitForSeconds(Time.deltaTime);
		}

		dragDistance *= -1;
		direction *= -1;
		controllable = true;
	}

	private void ChangeCharacter() {

		changing = true;
		if(transform.position.x < -2f)
			StartCoroutine( CharacterSpawn(0f));
		else
			StartCoroutine( CharacterSpawn(10f));
	}

	private IEnumerator CharacterSpawn (float target) {

		animator.SetTrigger ("move");
		while (transform.position.x < target) {
			transform.position += direction * speed * Time.deltaTime;
			if(transform.position.x > rightBorder.x)
				Destroy(gameObject);
			yield return new WaitForSeconds(Time.deltaTime);
		}
		animator.SetTrigger ("stop");
		GameManager.instance.changeCharacter = false;
		changing = false;
	}

	private void StartMove() {
		moveTrigger = true;
		death = false;
		animator.SetTrigger ("move");
	}
}
