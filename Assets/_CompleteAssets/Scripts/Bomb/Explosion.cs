using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public Rigidbody2D bombRb;
	public float jumpForce = 1f;
	public float maxExplodeDist = 0.5f;
	public Sprite redBomb;
	public AudioClip explosion;

	Animator explode;
	float explodeDist;
	bool rebote = false;
	bool dangerous = false;

	void Awake() {
		bombRb = GetComponent<Rigidbody2D> ();
		explode = GetComponent<Animator> ();
	}

	void OnCollisionEnter2D(Collision2D coll) {

		dangerous = true;
		transform.position = new Vector3(transform.position.x
		                                 ,transform.position.y
		                                 ,transform.position.z);
		explode.SetTrigger("explode");
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (dangerous && coll.gameObject.name == "Player" && !GameManager.instance.gameOver) {
			dangerous = false;
			SoundManager.instance.RandomizeSfx(explosion);
			GameManager.instance.GameOver();
		}
	}

	public void ExplosionEnds() {
		GameObject.Destroy(gameObject);
	}

	public void NotDangerous() {
		dangerous = false;
		GameManager.instance.DodgeBomb();
	}
}
